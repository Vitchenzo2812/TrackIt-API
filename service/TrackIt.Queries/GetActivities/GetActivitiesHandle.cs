using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Queries.GetActivities;

public class GetActivitiesHandle : IRequestHandler<GetActivitiesQuery, List<ActivityView>>
{
  private readonly TrackItDbContext _db;

  public GetActivitiesHandle (TrackItDbContext db)
  {
    _db = db;
  }

  public async Task<List<ActivityView>> Handle (GetActivitiesQuery request, CancellationToken cancellationToken)
  {
    var activities = await _db.Activity
      .Where(a => a.ActivityGroupId == request.Params.ActivityGroupId)
      .ToListAsync();

    return activities.Select(ActivityView.Build).ToList();
  }
}