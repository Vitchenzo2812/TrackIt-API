using TrackIt.Entities.Repository;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetActivityGroup;

public class GetActivityGroupHandle : IRequestHandler<GetActivityGroupQuery, GetActivityGroupResult>
{
  private readonly IActivityGroupRepository _activityGroupRepository;

  public GetActivityGroupHandle (
    IActivityGroupRepository activityGroupRepository
  )
  {
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<GetActivityGroupResult> Handle (GetActivityGroupQuery request, CancellationToken cancellationToken)
  {
    var activityGroup = await _activityGroupRepository.FindById(request.Params.ActivityGroupId);

    if (activityGroup is null)
      throw new NotFoundError("Activity Group not found");
    
    return GetActivityGroupResult.Build(activityGroup);
  }
}