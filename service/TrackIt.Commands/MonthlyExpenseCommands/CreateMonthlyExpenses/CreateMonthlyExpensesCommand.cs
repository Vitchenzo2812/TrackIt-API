using TrackIt.Entities.Core;

namespace TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpenses;

public class CreateMonthlyExpensesCommand (CreateMonthlyExpensesPayload payload, Session? session = null)
  : Command<CreateMonthlyExpensesPayload>(payload, session);