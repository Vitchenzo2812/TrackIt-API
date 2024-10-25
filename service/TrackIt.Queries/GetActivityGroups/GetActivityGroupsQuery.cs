using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetActivityGroups;

public class GetActivityGroupsQuery(Session? session = null)
  : Query<object, List<GetActivityGroupsResult>>(null, session);