using TrackIt.Entities.Core;

namespace TrackIt.Commands.MonthlyExpensesCommands.DeleteMonthlyExpenses;

public class DeleteMonthlyExpensesCommand(Guid aggregateId, Session? session = null)
  : Command<Guid, object>(aggregateId, null, session);