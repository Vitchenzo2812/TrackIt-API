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
  
  private async Task<MonthlyExpenses> CreateMonthlyExpenses (Guid userId, DateTime date)
  {
    var monthlyExpense = MonthlyExpenses.Create(userId, date);

    _db.MonthlyExpenses.Add(monthlyExpense);
    await _db.SaveChangesAsync();

    return monthlyExpense;
  }
}