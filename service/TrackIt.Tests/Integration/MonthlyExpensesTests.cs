using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config;
using System.Net;
using TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpenses;
using TrackIt.Commands.MonthlyExpenseCommands.UpdateMonthlyExpenses;
using TrackIt.Tests.Mocks.Entities;

namespace TrackIt.Tests.Integration;

public class MonthlyExpensesTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private MonthlyExpensesMock monthly { get; set; }
  
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

  [Fact]
  public async Task ShouldUpdateMonthlyExpenses ()
  {
    var user = await CreateUserWithEmailValidated();
    
    AddAuthorizationData(PartialSession.Create(user));

    await CreateMonthlyExpenses(user.Id);
    
    var payload = new UpdateMonthlyExpensesPayload(
      Title: "Diário de despesas 2",
      Description: "Descrição referente ao diário"
    );

    var response = await _httpClient.PutAsync($"/monthly-expense/{monthly.Id}", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var updated = await _db.MonthlyExpenses
      .Include(m => m.Expenses)
      .FirstOrDefaultAsync(m => m.Id == monthly.Id);
    
    Assert.NotNull(updated);
    
    Assert.Equal(payload.Title, updated.Title);
    Assert.Equal(payload.Description, updated.Description);
    Assert.Equal(user.Id, updated.UserId);
    Assert.Equal(DateTime.Parse("2024-08-14T00:00:00"), updated.Date);
  }

  [Fact]
  public async Task ShouldDeleteMonthlyExpenses ()
  {
    var user = await CreateUserWithEmailValidated();
    
    AddAuthorizationData(PartialSession.Create(user));

    await CreateMonthlyExpenses(user.Id);
    
    var response = await _httpClient.DeleteAsync($"/monthly-expense/{monthly.Id}");
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var deleted = await _db.MonthlyExpenses
      .Include(m => m.Expenses)
      .FirstOrDefaultAsync(m => m.Id == monthly.Id);
    
    Assert.Null(deleted);
  }
  
  private async Task CreateMonthlyExpenses (Guid userId)
  {
    monthly = new MonthlyExpensesMock()
      .ChangeTitle("Diário de despesas")
      .ChangeDescription("Uma descrição qualquer")
      .ChangeDate(DateTime.Parse("2024-08-14T00:00:00"))
      .AssignToUser(userId);

    _db.MonthlyExpenses.Add(monthly);
    await _db.SaveChangesAsync();
  }
}