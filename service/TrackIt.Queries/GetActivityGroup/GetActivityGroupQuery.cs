using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetActivityGroup;

public class GetActivityGroupQuery (GetActivityGroupParams @params, Session? session = null)
  : Query<GetActivityGroupParams, GetActivityGroupResult>(@params, session);