using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Core;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.Auth.SignUp;

public class SignUpHandle : IRequestHandler<SignUpCommand>
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
  
  public async Task Handle (SignUpCommand request, CancellationToken cancellationToken)
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
          _ticketRepository.Update(oldTicket);

          await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
          if (!oldTicket.CanReSend())
            throw;
          
          oldTicket.SendEmailVerification();
          return;
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
    
    if (oldUser is not null)
      _userRepository.Update(oldUser);
    else
      _userRepository.Save(user);
    
    _ticketRepository.Save(ticket);
    
    ticket.SendEmailVerification();
    await _unitOfWork.SaveChangesAsync();
  }
}