using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Infraestructure.Security.Models;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.Auth.SignIn;

public class SignInHandle : IRequestHandler<SignInCommand, Session>
{
  private readonly IUserRepository _userRepository;

  private readonly ISessionService _sessionService;
  
  public SignInHandle (
    IUserRepository userRepository,
    ISessionService sessionService
  )
  {
    _userRepository = userRepository;
    _sessionService = sessionService;
  }
  
  public async Task<Session> Handle (SignInCommand request, CancellationToken cancellationToken)
  {
    var user = _userRepository.FindByEmail(Email.FromAddress(request.Payload.Email));

    if (user is null)
      throw new NotFoundError("User not found");
    
    if (!Password.Verify(request.Payload.Password, user.Password!))
      throw new WrongEmailOrPasswordError();

    return await _sessionService.Create(user.Id);
  }
}