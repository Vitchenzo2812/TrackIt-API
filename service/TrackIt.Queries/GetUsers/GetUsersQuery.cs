using TrackIt.Entities.Core;
using TrackIt.Queries.Views;

namespace TrackIt.Queries.GetUsers;

public class GetUsersQuery (GetUsersParams @params, Session? session = null)
  : Query<GetUsersParams, PaginationView<List<UserResourceView>>>(@params, session);