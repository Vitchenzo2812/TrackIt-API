using TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpense;
using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration;

public class MonthlyExpensesTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldCreateMonthlyExpenses ()
  {
    var user = await CreateUserWithEmailValidated();
    
    AddAuthorizationData(PartialSession.Create(user));

    var payload = new CreateMonthlyExpensesPayload(
      Title: "Diário de despesas",
      Description: "Descrição referente ao diário"
    );

    var response = await _httpClient.PostAsync("/monthly-expense", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var created = await _db.MonthlyExpenses
      .Include(m => m.Expenses)
      .FirstOrDefaultAsync(m => m.UserId == user.Id);
    
    Assert.NotNull(created);
    
    Assert.Equal(payload.Title, created.Title);
    Assert.Equal(payload.Description, created.Description);
    Assert.Equal(user.Id, created.UserId);
  }
}