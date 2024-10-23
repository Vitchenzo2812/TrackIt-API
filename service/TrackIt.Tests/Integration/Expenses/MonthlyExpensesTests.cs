using TrackIt.Commands.MonthlyExpensesCommands.UpdateMonthlyExpenses;
using TrackIt.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config.Builders;
using TrackIt.Entities.Expenses;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Expenses;

public class MonthlyExpensesTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldUpdateMonthlyExpenses ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    var date = DateTime.Parse("2024-10-23T00:00:00");
    
    var monthlyExpense = await CreateMonthlyExpenses(user.Id, date);

    var payload = new UpdateMonthlyExpensesPayload(
      Title: "MONTHLY_EXPENSE_1",
      Limit: 5
    );
    
    var response = await _httpClient.PutAsync($"/monthly-expenses/{monthlyExpense.Id}", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var updated = await _db.MonthlyExpenses.ToListAsync();

    Assert.Single(updated);
    
    Assert.Equal(payload.Title, updated[0].Title);
    Assert.Equal(payload.Limit, updated[0].Limit);
    Assert.Equal(user.Id, updated[0].UserId);
    Assert.Equal(date, updated[0].Date);
  }

  [Fact]
  public async Task ShouldDeleteMonthlyExpenses ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    var date = DateTime.Parse("2024-10-10T00:00:00");
    
    var monthlyExpense = await CreateMonthlyExpenses(user.Id, date);
    await CreateExpenses(monthlyExpense.Id);

    var existingMonthlyExpenses = await _db.MonthlyExpenses.ToListAsync();
    var existingExpenses = await _db.Expenses.ToListAsync();
    
    Assert.Single(existingMonthlyExpenses);
    Assert.Equal(2, existingExpenses.Count);
    
    var response = await _httpClient.DeleteAsync($"/monthly-expenses/{monthlyExpense.Id}");
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var deletedMonthlyExpenses = await _db.MonthlyExpenses.ToListAsync();
    var deletedExpenses = await _db.Expenses.ToListAsync();
    
    Assert.Empty(deletedMonthlyExpenses);
    Assert.Empty(deletedExpenses);
  }
  
  private async Task<MonthlyExpenses> CreateMonthlyExpenses (Guid userId, DateTime date)
  {
    var monthlyExpense = MonthlyExpenses.Create(userId, date);

    _db.MonthlyExpenses.Add(monthlyExpense);
    await _db.SaveChangesAsync();

    return monthlyExpense;
  }

  private async Task CreateExpenses (Guid monthlyExpensesId)
  {
    var paymentFormat1 = PaymentFormat.Create()
      .WithTitle("Débito")
      .WithKey(PaymentFormatKey.DEBIT_CARD);

    var paymentFormatConfig1 = PaymentFormatConfig.Create()
      .WithIcon("ICON")
      .WithIconColor("ICON_COLOR")
      .WithBackgroundIconColor("BACKGROUND_ICON_COLOR")
      .AssignToPaymentFormat(paymentFormat1.Id);

    _db.PaymentFormats.Add(paymentFormat1);
    _db.PaymentFormatConfigs.Add(paymentFormatConfig1);
    
    var category1 = Category.Create()
      .WithTitle("Educação")
      .WithDescription("Descrição qualquer");
    
    var categoryConfig1 = CategoryConfig.Create()
      .WithIcon("ICON")
      .WithIconColor("ICON_COLOR")
      .WithBackgroundIconColor("BACKGROUND_ICON_COLOR")
      .AssignToCategory(category1.Id);

    _db.Categories.Add(category1);
    _db.CategoryConfigs.Add(categoryConfig1);
    
    var expense1 = Expense.Create()
      .WithTitle("EXPENSE_1")
      .WithDescription("EXPENSE_1_DESCRIPTION")
      .WithDate(DateTime.Parse("2024-10-20T00:00:00"))
      .WithAmount(500)
      .IsRecurringExpense(false)
      .AssignToPaymentFormat(paymentFormat1.Id)
      .AssignToCategory(category1.Id)
      .AssignToMonthly(monthlyExpensesId);
    
    var expense2 = Expense.Create()
      .WithTitle("EXPENSE_2")
      .WithDescription("EXPENSE_2_DESCRIPTION")
      .WithDate(DateTime.Parse("2024-10-19T00:00:00"))
      .WithAmount(78.16)
      .IsRecurringExpense(false)
      .AssignToPaymentFormat(paymentFormat1.Id)
      .AssignToCategory(category1.Id)
      .AssignToMonthly(monthlyExpensesId);

    _db.Expenses.AddRange([expense1, expense2]);
    await _db.SaveChangesAsync();
  }
}