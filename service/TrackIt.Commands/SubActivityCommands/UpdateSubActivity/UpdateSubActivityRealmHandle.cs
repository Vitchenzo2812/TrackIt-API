using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.SubActivityCommands.UpdateSubActivity;

public class UpdateSubActivityRealmHandle : IPipelineBehavior<UpdateSubActivityCommand, Unit>
{
  private readonly IUserRepository _userRepository;

  private readonly IActivityRepository _activityRepository;

  private readonly IActivityGroupRepository _activityGroupRepository;

  public UpdateSubActivityRealmHandle (
    IUserRepository userRepository,
    IActivityRepository activityRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityRepository = activityRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<Unit> Handle (UpdateSubActivityCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();
      
    var activityGroup = await _activityGroupRepository.FindById(request.Aggregate.ActivityGroupId);

    if (activityGroup is null)
      throw new NotFoundError("Activity group not found");

    var activity = await _activityRepository.FindById(request.Aggregate.ActivityId);
    
    if (activity is null)
      throw new NotFoundError("Activity not found");
    
    if (!activity.SubActivities.Any(x => x.Id == request.Aggregate.SubActivityId))
      throw new NotFoundError("Sub Activity not found");
    
    if (request.Session.Id == activityGroup.UserId)
      return await next();
    
    throw new ForbiddenError();
  }
}