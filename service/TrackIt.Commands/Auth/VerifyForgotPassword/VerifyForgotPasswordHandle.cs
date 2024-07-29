using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using TrackIt.Entities.Core;
using MediatR;

namespace TrackIt.Commands.Auth.VerifyForgotPassword;

public class VerifyForgotPasswordHandle : IRequestHandler<VerifyForgotPasswordCommand>
{
  private readonly IUserRepository _userRepository;
  
  private readonly ITicketRepository _ticketRepository;

  private readonly IUnitOfWork _unitOfWork;
  
  public VerifyForgotPasswordHandle (
    IUserRepository userRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork
  )
  {
    _userRepository = userRepository;
    _ticketRepository = ticketRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (VerifyForgotPasswordCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.FindById(request.Payload.UserId);

    if (user is null)
      throw new NotFoundError("User not found");
    
    var ticket = await _ticketRepository.FindLastWithTypeAndSituation(
      request.Payload.UserId, TicketType.RESET_PASSWORD, TicketSituation.OPEN
    );

    if (ticket is null)
      throw new NotFoundError("Ticket not found");
    
    ticket.Close(request.Payload.Code);

    await _unitOfWork.SaveChangesAsync();
  }
}