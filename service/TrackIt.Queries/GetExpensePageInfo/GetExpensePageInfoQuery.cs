using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetExpensePageInfo;

public class GetExpensePageInfoQuery (GetExpensePageInfoParams @params, Session? session = null)
  : Query<GetExpensePageInfoParams, List<GetExpensePageInfoResult>>(@params, session);