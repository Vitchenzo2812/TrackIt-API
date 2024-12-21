using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetSubActivity;

public class GetSubActivityQuery (GetSubActivityParams @params, Session? session = null)
  : Query<GetSubActivityParams, GetSubActivityResult>(@params, session);