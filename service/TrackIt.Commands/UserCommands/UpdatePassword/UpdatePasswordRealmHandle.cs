using MediatR;
using TrackIt.Entities.Errors;
using TrackIt.Infraestructure.Repository.Contracts;

namespace TrackIt.Commands.UserCommands.UpdatePassword;

public class UpdatePasswordRealmHandle : IPipelineBehavior<UpdatePasswordCommand, Unit>
{
  private readonly IUserRepository _userRepository;

  public UpdatePasswordRealmHandle (
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
  }
  
  public async Task<Unit> Handle (UpdatePasswordCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    if (await _userRepository.FindById(request.Session.Id) is null)
      throw new NotFoundError("User not found");

    return await next();
  }
}