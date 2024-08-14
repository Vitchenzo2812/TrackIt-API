using TrackIt.Entities;

namespace TrackIt.Commands.ExpenseCommands.CreateExpense;

public record CreateExpensePayload (
  DateTime Date,
  
  PaymentFormat PaymentFormat, 
  
  double Amount,
  
  string Description
);