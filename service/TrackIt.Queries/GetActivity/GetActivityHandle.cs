using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Errors;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Queries.GetActivity;

public class GetActivityHandle : IRequestHandler<GetActivityQuery, ActivityView>
{
  private readonly TrackItDbContext _db;

  public GetActivityHandle (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<ActivityView> Handle (GetActivityQuery request, CancellationToken cancellationToken)
  {
    var activity = await _db.Activity.FirstOrDefaultAsync(a => a.Id == request.Params.ActivityId);

    if (activity is null)
      throw new NotFoundError("Activity not found");
      
    return ActivityView.Build(activity);
  }
}