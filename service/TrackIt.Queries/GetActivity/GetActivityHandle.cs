using TrackIt.Entities.Repository;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetActivity;

public class GetActivityHandle : IRequestHandler<GetActivityQuery, GetActivityResult>
{
  private readonly IActivityRepository _activityRepository;

  public GetActivityHandle (
    IActivityRepository activityRepository
  )
  {
    _activityRepository = activityRepository;
  }
  
  public async Task<GetActivityResult> Handle (GetActivityQuery request, CancellationToken cancellationToken)
  {
    var activity = await _activityRepository.FindById(request.Params.ActivityId);

    if (activity is null)
      throw new NotFoundError("Activity not found");

    return GetActivityResult.Build(activity);
  }
}