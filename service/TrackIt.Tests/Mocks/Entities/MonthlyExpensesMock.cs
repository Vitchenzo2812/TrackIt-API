using TrackIt.Entities;
using TrackIt.Tests.Mocks.Contracts;

namespace TrackIt.Tests.Mocks.Entities;

public class MonthlyExpensesMock : MonthlyExpenses, IMock<MonthlyExpenses>
{
  public MonthlyExpensesMock AssignToUser (Guid userId)
  {
    UserId = userId;

    return this;
  }

  public MonthlyExpensesMock ChangeTitle (string title)
  {
    Title = title;

    return this;
  }
  
  public MonthlyExpensesMock ChangeDescription (string description)
  {
    Description = description;

    return this;
  }

  public MonthlyExpensesMock ChangeDate (DateTime date)
  {
    Date = date;

    return this;
  }
  
  public void Verify (MonthlyExpenses expect, MonthlyExpenses current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Description, current.Description);
    Assert.Equal(expect.Date, current.Date);
    Assert.Equal(expect.UserId, current.UserId);
    
    foreach (var expenseExpect in expect.Expenses)
    {
      var expenseCurrent = current.Expenses.Find(s => s.Id == expenseExpect.Id);
      
      Assert.NotNull(expenseCurrent);
      new ExpenseMock().Verify(expenseExpect, expenseCurrent);
    }
    
  }
}