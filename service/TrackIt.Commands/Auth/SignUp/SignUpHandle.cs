using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.Auth.SignUp;

public class SignUpHandle : IRequestHandler<SignUpCommand>
{
  private readonly IUserRepository _userRepository;
 
  private readonly IUnitOfWork _unitOfWork;
  
  public SignUpHandle (
    IUserRepository userRepository,
    ISessionService sessionService,
    IUnitOfWork unitOfWork
  )
  {
    _userRepository = userRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (SignUpCommand request, CancellationToken cancellationToken)
  {
    if (_userRepository.FindByEmail(Email.FromAddress(request.Payload.Email)) is not null)
      throw new EmailAlreadyInUseError();
    
    _userRepository.Save(
      User.Create(
      email: Email.FromAddress(request.Payload.Email),
      password: Password.Create(request.Payload.Password)
    ));

    await _unitOfWork.SaveChangesAsync();
  }
}