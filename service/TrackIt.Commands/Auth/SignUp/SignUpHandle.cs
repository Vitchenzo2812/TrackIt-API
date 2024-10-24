﻿using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Core;
using TrackIt.Entities;
using MediatR;
using TrackIt.Entities.Repository;

namespace TrackIt.Commands.Auth.SignUp;

public class SignUpHandle : IRequestHandler<SignUpCommand, SignUpResponse>
{
  private readonly IUserRepository _userRepository;

  private readonly ITicketRepository _ticketRepository;
  
  private readonly IUnitOfWork _unitOfWork;
  
  public SignUpHandle (
    IUserRepository userRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork
  )
  {
    _userRepository = userRepository;
    _ticketRepository = ticketRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task<SignUpResponse> Handle (SignUpCommand request, CancellationToken cancellationToken)
  {
    var oldUser = _userRepository.FindByEmail(Email.FromAddress(request.Payload.Email));

    var oldUserWasNotValidateEmail = (oldUser is not null && !oldUser.EmailValidated &&
                                     Password.Verify(request.Payload.Password, oldUser.Password!));
    
    if (oldUserWasNotValidateEmail)
    {
      var oldTicket = await _ticketRepository.FindLastWithTypeAndSituation(
        oldUser!.Id, TicketType.EMAIL_VERIFICATION, TicketSituation.OPEN
      );

      if (oldTicket is not null)
      {
        try
        {
          oldTicket.Cancel();

          await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
          if (!oldTicket.CanReSend())
            throw;
          
          oldTicket.SendEmailVerification();
          return new SignUpResponse(UserId: oldUser.Id);
        }
      }
    }
    
    var user = oldUser is not null 
      ? oldUser.SignUp(request.Payload.Email, request.Payload.Password) 
      : User.Create(
          Email.FromAddress(request.Payload.Email),
          Password.Create(request.Payload.Password)
        );
    
    var ticket = Ticket.Create(user.Id, TicketType.EMAIL_VERIFICATION, request.Payload.Email);
    
    if (oldUser is null)
      _userRepository.Save(user);
    
    _ticketRepository.Save(ticket);
    
    ticket.SendEmailVerification();
    await _unitOfWork.SaveChangesAsync();

    return new SignUpResponse(UserId: user.Id);
  }
}