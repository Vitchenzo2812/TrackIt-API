using TrackIt.Entities.Core;

namespace TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpense;

public class CreateMonthlyExpensesCommand (CreateMonthlyExpensesPayload payload, Session? session = null)
  : Command<CreateMonthlyExpensesPayload>(payload, session);