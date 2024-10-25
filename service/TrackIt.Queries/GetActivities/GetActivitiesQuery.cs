using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetActivities;

public class GetActivitiesQuery (GetActivitiesParams @params, Session? session = null)
  : Query<GetActivitiesParams, List<GetActivitiesResult>>(@params, session);