using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class Expense : Entity
{
  public Guid MonthlyExpensesId { get; set; }
  
  public DateTime Date { get; set; }

  public PaymentFormat PaymentFormat { get; set; }
  
  public double Amount { get; set; }

  public string Description { get; set; } = string.Empty;

  public static Expense Create (
    Guid monthlyExpensesId,
    DateTime date,
    PaymentFormat paymentFormat,
    double amount,
    string description
  )
  {
    return new Expense
    {
      MonthlyExpensesId = monthlyExpensesId,
      Date = date,
      PaymentFormat = paymentFormat,
      Amount = amount,
      Description = description
    };
  }
}