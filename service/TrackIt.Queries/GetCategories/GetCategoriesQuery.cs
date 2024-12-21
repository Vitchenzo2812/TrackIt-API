using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetCategories;

public class GetCategoriesQuery (Session? session = null)
  : Query<object, List<GetCategoriesResult>>(null, session);