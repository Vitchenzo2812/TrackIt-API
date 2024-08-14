using TrackIt.Entities.Core;

namespace TrackIt.Commands.ExpenseCommands.CreateExpense;

public class CreateExpenseCommand (Guid aggregateId, CreateExpensePayload payload, Session? session = null)
  : Command<Guid, CreateExpensePayload>(aggregateId, payload, session);