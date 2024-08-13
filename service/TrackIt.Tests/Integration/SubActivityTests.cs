using TrackIt.Commands.SubActivityCommands.CreateSubActivity;
using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Mocks.Entities;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration;

public class SubActivityTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private ActivityGroupMock group { get; set; }
  
  private ActivityMock activity { get; set; }
  
  [Fact]
  public async Task ShouldCreateSubActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    
    AddAuthorizationData(PartialSession.Create(user));

    await CreateActivityGroups(user.Id);
    await CreateActivities();

    var payload = new CreateSubActivityPayload(
      Title: "Sub Tarefa 1",
      Description: "Descrição da Sub Tarefa 1",
      Checked: true
    );
    
    var response = await _httpClient.PostAsync($"/activity-group/{group.Id}/activity/{activity.Id}/subActivity", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var created = await _db.Activity
      .Include(a => a.SubActivities)
      .FirstOrDefaultAsync(a => a.Id == activity.Id);
    
    Assert.NotNull(created);
    
    Assert.Single(created.SubActivities);
    Assert.Equal(1, created.SubActivities[0].Order);
    Assert.Equal(payload.Title, created.SubActivities[0].Title);
    Assert.Equal(payload.Checked, created.SubActivities[0].Checked);
    Assert.Equal(payload.Description, created.SubActivities[0].Description);
  }
  
  private async Task CreateActivities ()
  {
    activity = new ActivityMock()
      .ChangeTitle("Tarefa 1")
      .ChangeDescription("Descrição para tarefa 1")
      .WithOrder(1)
      .AssignToGroup(group.Id);
    
    _db.Activity.Add(activity);
    await _db.SaveChangesAsync();
  }
  
  private async Task CreateActivityGroups (Guid userId)
  {
    group = new ActivityGroupMock()
      .ChangeTitle("Grupo de Tarefas 1")
      .ChangeIcon("Icon 1")
      .WithOrder(1)
      .AssignToUser(userId);

    _db.ActivityGroup.Add(group);
    await _db.SaveChangesAsync();
  }
}