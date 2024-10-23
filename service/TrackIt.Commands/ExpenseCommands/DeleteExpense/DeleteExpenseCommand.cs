using TrackIt.Entities.Core;

namespace TrackIt.Commands.ExpenseCommands.DeleteExpense;

public class DeleteExpenseCommand(Guid aggregateId, Session? session = null)
  : Command<Guid, object>(aggregateId, null, session);