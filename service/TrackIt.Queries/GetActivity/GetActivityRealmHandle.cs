using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Queries.GetActivity;

public class GetActivityRealmHandle : IPipelineBehavior<GetActivityQuery, ActivityView>
{
  private readonly IUserRepository _userRepository;

  private readonly IActivityGroupRepository _activityGroupRepository;
  
  private readonly IActivityRepository _activityRepository;

  public GetActivityRealmHandle (
    IUserRepository userRepository,
    IActivityRepository activityRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityRepository = activityRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<ActivityView> Handle (GetActivityQuery request, RequestHandlerDelegate<ActivityView> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    var activityGroup = await _activityGroupRepository.FindById(request.Params.ActivityGroupId);
    
    if (activityGroup is null)
      throw new NotFoundError("Activity group not found");

    if (await _activityRepository.FindById(request.Params.ActivityId) is null)
      throw new NotFoundError("Activity not found");
    
    if (activityGroup.UserId == request.Session.Id)
      return await next();
    
    throw new ForbiddenError();
  }
}