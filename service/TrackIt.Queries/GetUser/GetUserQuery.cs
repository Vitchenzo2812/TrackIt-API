using TrackIt.Entities.Core;
using TrackIt.Queries.Views;

namespace TrackIt.Queries.GetUser;

public class GetUserQuery (GetUserParams @params, Session? session = null)
  : Query<GetUserParams, UserView>(@params, session);