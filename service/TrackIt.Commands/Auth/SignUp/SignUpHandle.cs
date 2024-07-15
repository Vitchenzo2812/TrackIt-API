using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Infraestructure.Security.Models;
using TrackIt.Commands.Errors;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.Auth.SignUp;

public class SignUpHandle : IRequestHandler<SignUpCommand, Session>
{
  private readonly IUserRepository _userRepository;
 
  private readonly IUnitOfWork _unitOfWork;

  private readonly ISessionService _sessionService;

  public SignUpHandle (
    IUserRepository userRepository,
    ISessionService sessionService,
    IUnitOfWork unitOfWork
  )
  {
    _userRepository = userRepository;
    _sessionService = sessionService;
    _unitOfWork = unitOfWork;
  }
  
  public async Task<Session> Handle (SignUpCommand request, CancellationToken cancellationToken)
  {
    if (_userRepository.FindByEmail(Email.FromAddress(request.Payload.Email)) is not null)
      throw new EmailAlreadyInUseError();

    var user = User.Create(
      email: Email.FromAddress(request.Payload.Email),
      password: Password.Create(request.Payload.Password)
    );
    
    _userRepository.Save(user);

    await _unitOfWork.SaveChangesAsync();
    
    return await _sessionService.Create(user.Id);
  }
}