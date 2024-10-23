using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;
using TrackIt.Entities.Repository;

namespace TrackIt.Commands.ActivityGroupCommands.DeleteActivityGroup;

public class DeleteActivityGroupRealmHandle : IPipelineBehavior<DeleteActivityGroupCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  private readonly IActivityGroupRepository _activityGroupRepository;

  public DeleteActivityGroupRealmHandle (
    IUserRepository userRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityGroupRepository = activityGroupRepository;
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

    var group = await _activityGroupRepository.FindById(request.Aggregate);

    if (group is null)
      throw new NotFoundError("Activity Group not found");

    if (group.UserId != user.Id)
      throw new ForbiddenError();
    
    return await next();
  }
}