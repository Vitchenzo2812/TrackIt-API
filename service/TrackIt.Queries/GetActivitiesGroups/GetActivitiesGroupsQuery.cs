using TrackIt.Entities.Core;
using TrackIt.Queries.Views;

namespace TrackIt.Queries.GetActivitiesGroups;

public class GetActivitiesGroupsQuery (GetActivitiesGroupsParams @params, Session? session = null)
  : Query<GetActivitiesGroupsParams, PaginationView<List<ActivityGroupView>>>(@params, session);