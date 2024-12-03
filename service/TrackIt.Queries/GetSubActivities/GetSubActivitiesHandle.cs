using TrackIt.Entities.Repository;
using MediatR;

namespace TrackIt.Queries.GetSubActivities;

public class GetSubActivitiesHandle : IRequestHandler<GetSubActivitiesQuery, List<GetSubActivitiesResult>>
{
  private readonly ISubActivityRepository _subActivityRepository;

  public GetSubActivitiesHandle (
    ISubActivityRepository subActivityRepository
  )
  {
    _subActivityRepository = subActivityRepository;
  }
  
  public async Task<List<GetSubActivitiesResult>> Handle (GetSubActivitiesQuery request, CancellationToken cancellationToken)
  {
    var subActivities = await _subActivityRepository.FindByActivityId(request.Params.ActivityId);

    return subActivities.Select(GetSubActivitiesResult.Build).ToList();
  }
}