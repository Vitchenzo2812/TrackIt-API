using TrackIt.Tests.Mocks.Entities;
using TrackIt.Entities;

namespace TrackIt.Tests.Unit;

public class ExpenseTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var expense = new ExpenseMock();
    
    Assert.Equal(0, expense.Amount);
    Assert.Equal(string.Empty, expense.Description);
  }
  
  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var monthlyExpensesId = Guid.NewGuid();

    var expense = new ExpenseMock()
      .ChangeDescription("Uma descrição qualquer")
      .ChangeDate(DateTime.Parse("2024-08-14T00:00:00"))
      .ChangeAmount(100)
      .ChangePaymentFormat(PaymentFormat.DEBIT)
      .AssignToMonthly(monthlyExpensesId);
    
    Assert.Equal("Uma descrição qualquer", expense.Description);
    Assert.Equal(DateTime.Parse("2024-08-14T00:00:00"), expense.Date);
    Assert.Equal(100, expense.Amount);
    Assert.Equal(PaymentFormat.DEBIT, expense.PaymentFormat);
    Assert.Equal(monthlyExpensesId, expense.MonthlyExpensesId);
  }
}