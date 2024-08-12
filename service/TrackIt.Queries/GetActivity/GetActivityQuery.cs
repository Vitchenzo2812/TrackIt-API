using TrackIt.Entities.Core;
using TrackIt.Queries.Views;

namespace TrackIt.Queries.GetActivity;

public class GetActivityQuery (GetActivityParams @params, Session? session = null)
  : Query<GetActivityParams, ActivityView>(@params, session);