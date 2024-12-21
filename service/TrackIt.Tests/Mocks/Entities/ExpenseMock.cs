using TrackIt.Entities.Expenses;
using TrackIt.Queries.GetExpense;

namespace TrackIt.Tests.Mocks.Entities;

public class ExpenseMock : Expense
{
  public static void Verify (GetExpenseResult expect, Expense current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Description, current.Description);
    Assert.Equal(expect.Date, current.Date);
    Assert.Equal(expect.Amount, current.Amount);
    Assert.Equal(expect.IsRecurring, current.IsRecurring);
    Assert.Equal(expect.CategoryId, current.CategoryId);
    Assert.Equal(expect.PaymentFormatId, current.PaymentFormatId);
  }
}