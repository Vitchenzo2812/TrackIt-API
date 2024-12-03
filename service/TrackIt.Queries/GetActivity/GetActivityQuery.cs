using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetActivity;

public class GetActivityQuery (GetActivityParams @params, Session? session = null)
  : Query<GetActivityParams, GetActivityResult>(@params, session);