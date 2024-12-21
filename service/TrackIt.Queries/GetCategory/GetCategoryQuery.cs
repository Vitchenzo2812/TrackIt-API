using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetCategory;

public class GetCategoryQuery (GetCategoryParams @params, Session? session = null)
  : Query<GetCategoryParams, GetCategoryResult>(@params, session);