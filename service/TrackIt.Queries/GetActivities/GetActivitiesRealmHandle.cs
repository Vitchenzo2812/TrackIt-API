using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Queries.GetActivities;

public class GetActivitiesRealmHandle : IPipelineBehavior<GetActivitiesQuery, List<ActivityView>>
{
  private readonly IUserRepository _userRepository;

  private readonly IActivityGroupRepository _activityGroupRepository;

  public GetActivitiesRealmHandle (
    IUserRepository userRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<List<ActivityView>> Handle (GetActivitiesQuery request, RequestHandlerDelegate<List<ActivityView>> next, CancellationToken cancellationToken)
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

    if (activityGroup.UserId == request.Session.Id)
      return await next();
    
    throw new ForbiddenError();
  }
}