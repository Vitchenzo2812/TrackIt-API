using TrackIt.Tests.Mocks.Contracts;
using TrackIt.Entities;

namespace TrackIt.Tests.Mocks.Entities;

public class ExpenseMock : Expense, IMock<Expense>
{
  public ExpenseMock AssignToMonthly (Guid monthlyExpensesId)
  {
    MonthlyExpensesId = monthlyExpensesId;

    return this;
  }

  public ExpenseMock ChangeDate (DateTime date)
  {
    Date = date;

    return this;
  }

  public ExpenseMock ChangePaymentFormat (PaymentFormat paymentFormat)
  {
    PaymentFormat = paymentFormat;

    return this;
  }

  public ExpenseMock ChangeAmount (double amount)
  {
    Amount = amount;

    return this;
  }

  public ExpenseMock ChangeDescription (string description)
  {
    Description = description;

    return this;
  }
  
  public void Verify (Expense expect, Expense current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Date, current.Date);
    Assert.Equal(expect.Amount, current.Amount);
    Assert.Equal(expect.Description, current.Description);
    Assert.Equal(expect.PaymentFormat, current.PaymentFormat);
    Assert.Equal(expect.MonthlyExpensesId, current.MonthlyExpensesId);
  }
}