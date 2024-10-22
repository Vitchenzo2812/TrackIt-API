using TrackIt.Entities.Expenses;
using TrackIt.Entities.Errors;

namespace TrackIt.Tests.Unit;

public class ExpenseTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var expense = Expense.Create();
    
    Assert.Null(expense.Description);
    Assert.False(expense.IsRecurring);
    Assert.Equal(string.Empty, expense.Title);
    Assert.Equal(0, expense.Amount);
    Assert.Equal(DateTime.MinValue, expense.Date);
    Assert.Equal(Guid.Empty, expense.CategoryId);
    Assert.Equal(Guid.Empty, expense.PaymentFormatId);
    Assert.Equal(Guid.Empty, expense.MonthlyExpensesId);
  }

  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var monthlyExpensesId = Guid.NewGuid();
    var categoryId = Guid.NewGuid();
    var paymentFormatId = Guid.NewGuid();
    
    var expense = Expense.Create()
      .WithTitle("EXPENSE_1")
      .WithDescription("EXPENSE_1_DESCRIPTION")
      .WithAmount(750)
      .WithDate(DateTime.Parse("2024-10-22T00:00:00"))
      .IsRecurringExpense(true)
      .AssignToMonthly(monthlyExpensesId)
      .AssignToCategory(categoryId)
      .AssignToPaymentFormat(paymentFormatId);
    
    Assert.True(expense.IsRecurring);
    Assert.Equal("EXPENSE_1", expense.Title);
    Assert.Equal("EXPENSE_1_DESCRIPTION", expense.Description);
    Assert.Equal(750, expense.Amount);
    Assert.Equal(DateTime.Parse("2024-10-22T00:00:00"), expense.Date);
    Assert.Equal(monthlyExpensesId, expense.MonthlyExpensesId);
    Assert.Equal(categoryId, expense.CategoryId);
    Assert.Equal(paymentFormatId, expense.PaymentFormatId);
  }

  [Theory]
  [InlineData(double.PositiveInfinity)]
  [InlineData(double.NegativeInfinity)]
  [InlineData(double.NaN)]
  [InlineData(-485)]
  public void ShouldThrowInvalidAmountValueError (double amount)
  {
    var expense = Expense.Create();
    
    var exception = Assert.Throws<InvalidAmountValueError>(() => expense.WithAmount(amount));
    
    Assert.Equal("Invalid amount value", exception.Message);
  }
  
  [Theory]
  [InlineData("5000-01-10T00:00:00")]
  [InlineData("0001-01-01T00:00:00")]
  [InlineData("9999-12-31T23:59:59")]
  public void ShouldThrowInvalidDateError (string dateString)
  {
    var date = DateTime.Parse(dateString);
      
    var expense = Expense.Create();
    
    var exception = Assert.Throws<InvalidDateError>(() => expense.WithDate(date));
    
    Assert.Equal("Invalid date", exception.Message);
  }
}