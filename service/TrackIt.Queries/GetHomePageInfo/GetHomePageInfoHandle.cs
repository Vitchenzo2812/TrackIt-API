using TrackIt.Infraestructure.Database;
using TrackIt.Queries.Views.HomePage;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Errors;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Queries.GetHomePageInfo;

public class GetHomePageInfoHandle : IRequestHandler<GetHomePageInfoQuery, HomePageView>
{
  private readonly TrackItDbContext _db;

  public GetHomePageInfoHandle (TrackItDbContext db) => _db = db;
  
  public async Task<HomePageView> Handle (GetHomePageInfoQuery request, CancellationToken cancellationToken)
  {
    var activityGroup = await _db.ActivityGroups
      .Include(x => x.Activities)
      .FirstOrDefaultAsync(x => x.Id == request.Params.ActivityGroupId);

    if (activityGroup is null)
      throw new NotFoundError("Activity group not found");

    var completedActivitiesQuery = activityGroup.Activities
      .Where(x => x.Checked)
      .ToList();
    
    var incompletedActivitiesQuery = activityGroup.Activities
      .Where(x => !x.Checked)
      .ToList();

    var totalActivities = activityGroup.Activities.Count;
    
    var percentageCompletedActivities = totalActivities > 0
      ? (int)Math.Round((completedActivitiesQuery.Count / (double)totalActivities) * 100)
      : 0;
    
    var completedActivities = completedActivitiesQuery
      .Take(request.Params.CompletedActivitiesPerPage)
      .Select(PartialActivityView.Build)
      .ToList();
    
    var incompletedActivities = incompletedActivitiesQuery
      .Take(request.Params.IncompletedActivitiesPerPage)
      .Select(PartialActivityView.Build)
      .ToList();
    
    var totalCompletedActivitiesPages = (int)Math.Ceiling((double)completedActivities.Count / request.Params.CompletedActivitiesPerPage);
    var totalIncompletedActivitiesPages = (int)Math.Ceiling((double)incompletedActivities.Count / request.Params.IncompletedActivitiesPerPage);
    
    return HomePageView.Build(
      percentageCompletedActivities,
      PaginationView<List<PartialActivityView>>.Build(1, totalCompletedActivitiesPages, completedActivities), 
      PaginationView<List<PartialActivityView>>.Build(1, totalIncompletedActivitiesPages, incompletedActivities) 
    );
  }
}