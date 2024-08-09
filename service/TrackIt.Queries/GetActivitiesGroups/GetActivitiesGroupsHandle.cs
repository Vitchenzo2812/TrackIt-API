using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Queries.GetActivitiesGroups;

public class GetActivitiesGroupsHandle 
  : IRequestHandler<GetActivitiesGroupsQuery, PaginationView<List<ActivityGroupView>>>
{
  private readonly TrackItDbContext _db;

  public GetActivitiesGroupsHandle (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<PaginationView<List<ActivityGroupView>>> Handle (GetActivitiesGroupsQuery request, CancellationToken cancellationToken)
  {
    var groupsQuery = await _db.ActivityGroup.ToListAsync(cancellationToken);
    
    var groups = groupsQuery
      .Skip((request.Params.Page - 1) * request.Params.PerPage)
      .Take(request.Params.PerPage)
      .Select(ActivityGroupView.Build)
      .ToList();
    
    var totalPages = (int)Math.Ceiling((double)groupsQuery.Count() / request.Params.PerPage);
    
    return PaginationView<List<ActivityGroupView>>.Build(request.Params.Page, totalPages, groups);
  }
}