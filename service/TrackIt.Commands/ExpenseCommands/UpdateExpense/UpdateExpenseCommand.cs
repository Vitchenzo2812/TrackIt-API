using TrackIt.Entities.Core;

namespace TrackIt.Commands.ExpenseCommands.UpdateExpense;

public class UpdateExpenseCommand(Guid aggregateId, UpdateExpensePayload payload, Session? session = null)
  : Command<Guid, UpdateExpensePayload>(aggregateId, payload, session);