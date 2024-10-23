using TrackIt.Entities.Core;

namespace TrackIt.Commands.MonthlyExpensesCommands.UpdateMonthlyExpenses;

public class UpdateMonthlyExpensesCommand(Guid aggregateId, UpdateMonthlyExpensesPayload payload, Session? session = null)
  : Command<Guid, UpdateMonthlyExpensesPayload>(aggregateId, payload, session);