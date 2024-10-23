using TrackIt.Commands.ExpenseCommands.CreateExpense;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Tests.Config.Builders;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Expenses;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Expenses;

public class ExpenseTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private PaymentFormat paymentFormat1 { get; set; }
  private PaymentFormat paymentFormat2 { get; set; }
  private PaymentFormat paymentFormat3 { get; set; }
  private Category category1 { get; set; }
  private Category category2 { get; set; }
  
  [Fact]
  public async Task ShouldCreateExpenseAndMonthly ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreatePaymentFormats();
    await CreateCategories();

    var date = DateTime.Parse("2024-09-10T00:10:00");
    
    var payload = new CreateExpensePayload(
      Title: "EXPENSE_1",
      Description: "EXPENSE_1_DESCRIPTION",
      Date: date,
      Amount: 150,
      PaymentFormatId: paymentFormat1.Id,
      CategoryId: category2.Id,
      IsRecurring: false
    );
    
    var response = await _httpClient.PostAsync("/expense", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var expense = await _db.Expenses.ToListAsync();
    var monthlyExpense = await _db.MonthlyExpenses.ToListAsync();
    
    Assert.Single(expense);
    Assert.Single(monthlyExpense);

    #region monthlyExpense validations

      Assert.Null(monthlyExpense[0].Limit);
      Assert.Equal(user.Id, monthlyExpense[0].UserId);
      Assert.Equal("Setembro de 2024", monthlyExpense[0].Title);
      Assert.Equal(date, monthlyExpense[0].Date);

    #endregion

    #region expense validations

      Assert.Equal(payload.Title, expense[0].Title);
      Assert.Equal(payload.Description, expense[0].Description);
      Assert.Equal(payload.Date, expense[0].Date);
      Assert.Equal(payload.Amount, expense[0].Amount);
      Assert.Equal(payload.IsRecurring, expense[0].IsRecurring);
      Assert.Equal(payload.CategoryId, expense[0].CategoryId);
      Assert.Equal(monthlyExpense[0].Id, expense[0].MonthlyExpensesId);
      Assert.Equal(payload.PaymentFormatId, expense[0].PaymentFormatId);

    #endregion
  }

  [Fact]
  public async Task ShouldCreateExpenseWithMonthlyAlreadyCreated ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreatePaymentFormats();
    await CreateCategories();

    var date = DateTime.Parse("2024-09-10T00:10:00");

    await CreateMonthlyExpenses(user.Id, date);
    
    var payload = new CreateExpensePayload(
      Title: "EXPENSE_1",
      Description: "EXPENSE_1_DESCRIPTION",
      Date: date,
      Amount: 150,
      PaymentFormatId: paymentFormat1.Id,
      CategoryId: category2.Id,
      IsRecurring: false
    );
    
    var response = await _httpClient.PostAsync("/expense", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var expense = await _db.Expenses.ToListAsync();
    var monthlyExpense = await _db.MonthlyExpenses.ToListAsync();
    
    Assert.Single(expense);
    Assert.Single(monthlyExpense);

    #region monthlyExpense validations

    Assert.Null(monthlyExpense[0].Limit);
    Assert.Equal(user.Id, monthlyExpense[0].UserId);
    Assert.Equal("Setembro de 2024", monthlyExpense[0].Title);
    Assert.Equal(date, monthlyExpense[0].Date);

    #endregion

    #region expense validations

    Assert.Equal(payload.Title, expense[0].Title);
    Assert.Equal(payload.Description, expense[0].Description);
    Assert.Equal(payload.Date, expense[0].Date);
    Assert.Equal(payload.Amount, expense[0].Amount);
    Assert.Equal(payload.IsRecurring, expense[0].IsRecurring);
    Assert.Equal(payload.CategoryId, expense[0].CategoryId);
    Assert.Equal(monthlyExpense[0].Id, expense[0].MonthlyExpensesId);
    Assert.Equal(payload.PaymentFormatId, expense[0].PaymentFormatId);

    #endregion
  }
  
  private async Task CreatePaymentFormats ()
  {
    paymentFormat1 = PaymentFormat.Create()
      .WithKey(PaymentFormatKey.MONEY)
      .WithTitle("Débito");

    var paymentFormatConfig1 = PaymentFormatConfig.Create()
      .WithIcon("/icon/debit_card.svg")
      .WithIconColor("#F4F4F4")
      .WithBackgroundIconColor("#120DF1")
      .AssignToPaymentFormat(paymentFormat1.Id);

    paymentFormat2 = PaymentFormat.Create()
      .WithKey(PaymentFormatKey.PAYMENT_SLIP)
      .WithTitle("Boleto");
    
    var paymentFormatConfig2 = PaymentFormatConfig.Create()
      .WithIcon("/icon/payment_slip.svg")
      .WithIconColor("#F4F4F4")
      .WithBackgroundIconColor("#9100D6")
      .AssignToPaymentFormat(paymentFormat2.Id);
    
    paymentFormat3 = PaymentFormat.Create()
      .WithKey(PaymentFormatKey.CREDIT_CARD)
      .WithTitle("Crédito");
    
    var paymentFormatConfig3 = PaymentFormatConfig.Create()
      .WithIcon("/icon/credit_card.svg")
      .WithIconColor("#F4F4F4")
      .WithBackgroundIconColor("#000000")
      .AssignToPaymentFormat(paymentFormat3.Id);

    _db.PaymentFormats.AddRange([paymentFormat1, paymentFormat2, paymentFormat3]);
    _db.PaymentFormatConfigs.AddRange([paymentFormatConfig1, paymentFormatConfig2, paymentFormatConfig3]);
    
    await _db.SaveChangesAsync();
  }
  
  private async Task CreateCategories ()
  {
    category1 = Category.Create()
      .WithTitle("Educação")
      .WithDescription("Faculdade, cursos etc.");
    
    var categoryConfig1 = CategoryConfig.Create()
      .WithIcon("/icon/school.svg")
      .WithIconColor("#F4F4F4")
      .WithBackgroundIconColor("#120DF1")
      .AssignToCategory(category1.Id);
    
    category2 = Category.Create()
      .WithTitle("Alimentação")
      .WithDescription("Fast-Food, Almoço em familia etc.");
    
    var categoryConfig2 = CategoryConfig.Create()
      .WithIcon("/icon/food.svg")
      .WithIconColor("#F4F4F4")
      .WithBackgroundIconColor("#000000")
      .AssignToCategory(category2.Id);

    _db.Categories.AddRange([category1, category2]);
    _db.CategoryConfigs.AddRange([categoryConfig1, categoryConfig2]);
    
    await _db.SaveChangesAsync();
  }

  private async Task CreateMonthlyExpenses (Guid userId, DateTime date)
  {
    var monthlyExpense = MonthlyExpenses.Create(userId, date);

    _db.MonthlyExpenses.Add(monthlyExpense);
    await _db.SaveChangesAsync();
  }
}