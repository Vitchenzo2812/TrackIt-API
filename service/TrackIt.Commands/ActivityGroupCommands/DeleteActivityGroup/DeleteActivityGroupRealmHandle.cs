using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.ActivityGroupCommands.DeleteActivityGroup;

public class DeleteActivityGroupRealmHandle : IPipelineBehavior<DeleteActivityGroupCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  
  public DeleteActivityGroupRealmHandle (IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }
  
  public async Task<Unit> Handle (DeleteActivityGroupCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();
    
    return await next();
  }
}