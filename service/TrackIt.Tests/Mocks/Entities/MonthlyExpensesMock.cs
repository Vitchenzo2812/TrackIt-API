using TrackIt.Queries.GetExpensePageInfo;
using TrackIt.Entities.Expenses;

namespace TrackIt.Tests.Mocks.Entities;

public class MonthlyExpensesMock : MonthlyExpenses
{
  public static void Verify (GetExpensePageInfoResult expect, MonthlyExpenses monthlyExpenses, int totalExpenses)
  {
    var percentageLimitExpenses = 0.0;

    if (monthlyExpenses.Limit is not null && monthlyExpenses.Limit != 0)
      percentageLimitExpenses = ((totalExpenses / (double)monthlyExpenses.Limit) * 100);
      
    Assert.Equal(expect.Title, monthlyExpenses.Title);
    Assert.Equal(expect.TotalExpenses, totalExpenses);
    Assert.Equal(expect.LimitExpenses, monthlyExpenses.Limit);
    Assert.Equal(expect.MonthlyExpenseId, monthlyExpenses.Id);
    Assert.Equal(expect.PercentageLimitExpenses, percentageLimitExpenses);
  }
}