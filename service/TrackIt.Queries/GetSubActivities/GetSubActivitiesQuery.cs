using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetSubActivities;

public class GetSubActivitiesQuery (GetSubActivitiesParams @params, Session? session = null)
  : Query<GetSubActivitiesParams, List<GetSubActivitiesResult>>(@params, session);