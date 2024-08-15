using TrackIt.Entities.Core;

namespace TrackIt.Commands.MonthlyExpenseCommands.DeleteMonthlyExpenses;

public class DeleteMonthlyExpensesCommand (Guid aggregateId, Session? session = null)
  : Command<Guid, object>(aggregateId, null, session);