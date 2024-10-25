using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace TrackIt.Queries.GetActivityGroups;

public class GetActivityGroupsHandle : IRequestHandler<GetActivityGroupsQuery, List<GetActivityGroupsResult>>
{
  private readonly TrackItDbContext _db;

  public GetActivityGroupsHandle (TrackItDbContext db) => _db = db;
  
  public async Task<List<GetActivityGroupsResult>> Handle (GetActivityGroupsQuery request, CancellationToken cancellationToken)
  {
    var activityGroups = await _db.ActivityGroups
      .Where(x => x.UserId == request.Session!.Id)
      .Select(x => new { x.Id, x.Title, x.Order })
      .ToListAsync();

    return activityGroups
      .Select(x => GetActivityGroupsResult.Build(x.Id, x.Title, x.Order))
      .ToList();
  }
}