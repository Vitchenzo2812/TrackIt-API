using TrackIt.Entities.Core;
using TrackIt.Entities.Errors;

namespace TrackIt.Entities.Expenses;

public class Expense : Aggregate
{
  public required string Title { get; set; }
  public required string? Description { get; set; }
  public required DateTime Date { get; set; }
  public required double Amount { get; set; }
  public required bool IsRecurring { get; set; }
  public required Guid CategoryId { get; set; }
  public required Guid MonthlyExpensesId { get; set; }
  public required Guid PaymentFormatId { get; set; }

  public static Expense Create ()
  {
    return new Expense
    {
      Title = string.Empty,
      Description = null,
      Date = default,
      Amount = 0,
      IsRecurring = false,
      CategoryId = default,
      MonthlyExpensesId = default,
      PaymentFormatId = default
    };
  }
  
  public Expense WithTitle (string title)
  {
    Title = title;
    return this;
  }
  
  public Expense WithDescription (string? description)
  {
    Description = description;
    return this;
  }

  public Expense WithAmount (double amount)
  {
    if (
      double.IsInfinity(amount) ||
      double.IsNaN(amount) ||
      amount < 0
    )
      throw new InvalidAmountValueError();
    
    Amount = amount;
    return this;
  }

  public Expense WithDate (DateTime date)
  {
    if (
      date > DateTime.Now ||
      date <= DateTime.MinValue || 
      date >= DateTime.MaxValue
    )
      throw new InvalidDateError();
    
    Date = date;
    return this;
  }
  
  public Expense IsRecurringExpense (bool isRecurring)
  {
    IsRecurring = isRecurring;
    return this;
  }

  public Expense AssignToPaymentFormat (Guid paymentFormatId)
  {
    PaymentFormatId = paymentFormatId;
    return this;
  }
  
  public Expense AssignToCategory (Guid categoryId)
  {
    CategoryId = categoryId;
    return this;
  }
  
  public Expense AssignToMonthly (Guid monthlyExpensesId)
  {
    MonthlyExpensesId = monthlyExpensesId;
    return this;
  }
}