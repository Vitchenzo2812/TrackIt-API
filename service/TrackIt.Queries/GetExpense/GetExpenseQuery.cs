using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetExpense;

public class GetExpenseQuery (GetExpenseParams @params, Session? session = null)
  : Query<GetExpenseParams, GetExpenseResult>(@params, session);