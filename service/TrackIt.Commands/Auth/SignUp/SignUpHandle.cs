using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.Auth.SignUp;

public class SignUpHandle (
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
  )
  : IRequestHandler<SignUpCommand, SignUpResponse>
{
  public async Task<SignUpResponse> Handle (SignUpCommand request, CancellationToken cancellationToken)
  {
    if (await userRepository.FindByEmail(Email.FromAddress(request.Payload.Email)) is not null)
      throw new EmailAlreadyInUseError();

    var user = User.Create(
      email: Email.FromAddress(request.Payload.Email),
      password: Password.Create(request.Payload.Password)
    );
    
    userRepository.Save(user);

    await unitOfWork.SaveChangesAsync();
    
    return new SignUpResponse(UserId: user.Id);
  }
}