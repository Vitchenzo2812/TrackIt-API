using TrackIt.Tests.Mocks.Entities;

namespace TrackIt.Tests.Unit;

public class MonthlyExpensesTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var monthlyExpenses = new MonthlyExpensesMock();
    
    Assert.Null(monthlyExpenses.Title);
    Assert.Null(monthlyExpenses.Description);
    Assert.Empty(monthlyExpenses.Expenses);
  }
  
  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var userId = Guid.NewGuid();
      
    var monthlyExpenses = new MonthlyExpensesMock()
      .ChangeTitle("Diário de gastos mensal")
      .ChangeDescription("Uma descrição qualquer")
      .ChangeDate(DateTime.Parse("2024-08-14T00:00:00"))
      .AssignToUser(userId);
    
    Assert.Equal("Diário de gastos mensal", monthlyExpenses.Title);
    Assert.Equal("Uma descrição qualquer", monthlyExpenses.Description);
    Assert.Equal(DateTime.Parse("2024-08-14T00:00:00"), monthlyExpenses.Date);
    Assert.Equal(userId, monthlyExpenses.UserId);
  }
}