using TrackIt.Entities.Expenses;

namespace TrackIt.Tests.Unit;

public class MonthlyExpensesTests
{
  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var userId = Guid.Empty;
    var date = DateTime.Parse("2024-10-22T00:00:00");
    var title = $"Outubro de 2024";
    
    var monthlyExpenses = MonthlyExpenses.Create(userId, date);
    
    Assert.Null(monthlyExpenses.Limit);
    Assert.Equal(userId, monthlyExpenses.UserId);
    Assert.Equal(date, monthlyExpenses.Date);
    Assert.Equal(title, monthlyExpenses.Title);
    
    var diffDate = DateTime.Parse("2020-01-22T00:00:00");
    var diffTitle = $"Janeiro de 2020";
    
    var diffMonthlyExpenses = MonthlyExpenses.Create(userId, diffDate);
    
    Assert.Null(diffMonthlyExpenses.Limit);
    Assert.Equal(userId, diffMonthlyExpenses.UserId);
    Assert.Equal(diffDate, diffMonthlyExpenses.Date);
    Assert.Equal(diffTitle, diffMonthlyExpenses.Title);
  }
}