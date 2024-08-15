using TrackIt.Entities.Core;
using TrackIt.Queries.Views;

namespace TrackIt.Queries.GetMonthlyExpenses;

public class GetMonthlyExpensesQuery (GetMonthlyExpensesParams @params, Session? session = null)
  : Query<GetMonthlyExpensesParams, List<MonthlyExpensesView>>(@params, session);