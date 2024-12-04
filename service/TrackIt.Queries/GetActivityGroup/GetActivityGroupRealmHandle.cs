using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetActivityGroup;

public class GetActivityGroupRealmHandle : IPipelineBehavior<GetActivityGroupQuery, GetActivityGroupResult>
{
  private readonly IUserRepository _userRepository;
  private readonly IActivityGroupRepository _activityGroupRepository;
  
  public GetActivityGroupRealmHandle (
    IUserRepository userRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<GetActivityGroupResult> Handle (GetActivityGroupQuery request, RequestHandlerDelegate<GetActivityGroupResult> next, CancellationToken cancellationToken)
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
    
    return await next();
  }
}