using TrackIt.Entities.Errors;
using TrackIt.Entities;
using MediatR;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Repository;

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
    var user = _userRepository.FindByEmail(Email.FromAddress(request.Payload.Email)); 
    
    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();
    
    return await next();
  }
}