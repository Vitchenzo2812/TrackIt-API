using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using TrackIt.Entities.Core;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.Auth.ForgotPassword;

public class ForgotPasswordHandle : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponse>
{
  private readonly IUserRepository _userRepository;

  private readonly ITicketRepository _ticketRepository;

  private readonly IUnitOfWork _unitOfWork;

  public ForgotPasswordHandle (
    IUserRepository userRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork
  )
  {
    _userRepository = userRepository;
    _ticketRepository = ticketRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task<ForgotPasswordResponse> Handle (ForgotPasswordCommand request, CancellationToken cancellationToken)
  {
    var user = _userRepository.FindByEmail(Email.FromAddress(request.Payload.Email));
    
    if (user is null)
      throw new NotFoundError("User not found");

    var oldTicket = await _ticketRepository.FindLastWithTypeAndSituation(
      user.Id, TicketType.RESET_PASSWORD, TicketSituation.OPEN
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
          
        oldTicket.SendEmailForgotPassword();
        return new ForgotPasswordResponse(UserId: user.Id);
      }
    }

    var ticket = Ticket.Create(user.Id, TicketType.RESET_PASSWORD, user.Email!.Value);
    
    _ticketRepository.Save(ticket);
    
    ticket.SendEmailForgotPassword();
    await _unitOfWork.SaveChangesAsync();
    
    return new ForgotPasswordResponse(UserId: user.Id);
  }
}