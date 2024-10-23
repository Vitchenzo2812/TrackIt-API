using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;
using TrackIt.Entities.Repository;

namespace TrackIt.Commands.SubActivityCommands.CreateSubActivity;

public class CreateSubActivityRealmHandle : IPipelineBehavior<CreateSubActivityCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  private readonly IActivityRepository _activityRepository;
  private readonly IActivityGroupRepository _activityGroupRepository;

  public CreateSubActivityRealmHandle (
    IUserRepository userRepository,
    IActivityRepository activityRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityRepository = activityRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<Unit> Handle (CreateSubActivityCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    var group = await _activityGroupRepository.FindById(request.Aggregate.GroupId);

    if (group is null)
      throw new NotFoundError("Activity Group not found");

    if (group.UserId != user.Id)
      throw new ForbiddenError("Activity group doesn't belong to this user");

    var activity = await _activityRepository.FindById(request.Aggregate.ActivityId);

    if (activity is null)
      throw new NotFoundError("Activity not found");
    
    if (activity.ActivityGroupId != group.Id)
      throw new ForbiddenError("Activity doesn't belong to this activity group");
    
    return await next();
  }
}