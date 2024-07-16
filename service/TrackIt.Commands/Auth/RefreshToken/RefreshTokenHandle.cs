using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Infraestructure.Security.Models;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.Auth.RefreshToken;

public class RefreshTokenHandle : IRequestHandler<RefreshTokenCommand, Session>
{
  private readonly IUserRepository _userRepository;

  private readonly ISessionService _sessionService;

  private readonly IJwtService _jwtService;

  private readonly IRefreshTokenService _refreshTokenService;

  public RefreshTokenHandle (
    IUserRepository userRepository,
    ISessionService sessionService,
    IJwtService jwtService,
    IRefreshTokenService refreshTokenService
  )
  {
    _refreshTokenService = refreshTokenService;
    _jwtService = jwtService;
    _sessionService = sessionService;
    _userRepository = userRepository;
  }
  
  public async Task<Session> Handle (RefreshTokenCommand request, CancellationToken cancellationToken)
  {
    var decoded = _jwtService.Decode(request.Payload.Token);
    var refreshToken = await _refreshTokenService.GetToken(decoded.Id);

    if (request.Payload.RefreshToken != refreshToken)
      throw new CannotRefreshToken();

    var user = await _userRepository.FindById(decoded.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    return await _sessionService.Create(user.Id);
  }
}