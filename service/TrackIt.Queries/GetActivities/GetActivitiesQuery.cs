using TrackIt.Entities.Core;
using TrackIt.Queries.Views;

namespace TrackIt.Queries.GetActivities;

public class GetActivitiesQuery (GetActivitiesParams @params, Session? session = null)
  : Query<GetActivitiesParams, List<ActivityView>>(@params, session);