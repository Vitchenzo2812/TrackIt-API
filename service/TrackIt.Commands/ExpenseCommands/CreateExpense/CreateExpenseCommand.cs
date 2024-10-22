using TrackIt.Entities.Core;

namespace TrackIt.Commands.ExpenseCommands.CreateExpense;

public class CreateExpenseCommand(CreateExpensePayload payload, Session? session = null)
  : Command<CreateExpensePayload>(payload, session);