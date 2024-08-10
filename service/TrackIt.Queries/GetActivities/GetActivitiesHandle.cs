using TrackIt.Infraestructure.Database;
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
    throw new NotImplementedException();
  }
}