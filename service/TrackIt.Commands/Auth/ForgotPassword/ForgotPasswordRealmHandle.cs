using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Entities.Errors;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.Auth.ForgotPassword;

public class ForgotPasswordRealmHandle : IPipelineBehavior<ForgotPasswordCommand, ForgotPasswordResponse>
{
  private readonly IUserRepository _userRepository;

  public ForgotPasswordRealmHandle (
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
  }
  
  public async Task<ForgotPasswordResponse> Handle (ForgotPasswordCommand request, RequestHandlerDelegate<ForgotPasswordResponse> next, CancellationToken cancellationToken)
  {
    if (_userRepository.FindByEmail(Email.FromAddress(request.Payload.Email)) is null)
      throw new NotFoundError("User not found");

    return await next();
  }
}