using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetSubActivity;

public class GetSubActivityRealmHandle : IPipelineBehavior<GetSubActivityQuery, GetSubActivityResult>
{
  private readonly IUserRepository _userRepository;
  private readonly IActivityRepository _activityRepository;
  private readonly ISubActivityRepository _subActivityRepository;
  private readonly IActivityGroupRepository _activityGroupRepository;

  public GetSubActivityRealmHandle (
    IUserRepository userRepository,
    IActivityRepository activityRepository,
    ISubActivityRepository subActivityRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityRepository = activityRepository;
    _subActivityRepository = subActivityRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<GetSubActivityResult> Handle (GetSubActivityQuery request, RequestHandlerDelegate<GetSubActivityResult> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    if (await _activityGroupRepository.FindById(request.Params.ActivityGroupId) is null)
      throw new NotFoundError("Activity Group not found");
    
    if (await _activityRepository.FindById(request.Params.ActivityId) is null)
      throw new NotFoundError("Activity not found");
    
    if (await _subActivityRepository.FindById(request.Params.SubActivityId) is null)
      throw new NotFoundError("SubActivity not found");
    
    return await next();
  }
}