using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace TrackIt.Queries.GetActivities;

public class GetActivitiesHandle : IRequestHandler<GetActivitiesQuery, List<GetActivitiesResult>>
{
  private readonly TrackItDbContext _db;

  public GetActivitiesHandle (TrackItDbContext db) => _db = db;
  
  public async Task<List<GetActivitiesResult>> Handle (GetActivitiesQuery request, CancellationToken cancellationToken)
  {
    var activities = await _db.Activities
      .Where(x => x.ActivityGroupId == request.Params.ActivityGroupId)
      .OrderBy(x => x.Order)
      .ToListAsync();

    return activities.Select(GetActivitiesResult.Build).ToList();
  }
}