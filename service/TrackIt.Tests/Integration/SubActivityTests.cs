using TrackIt.Commands.SubActivityCommands.CreateSubActivity;
using TrackIt.Commands.SubActivityCommands.UpdateSubActivity;
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
  
  private SubActivityMock subActivity { get; set; }
  
  [Fact]
  public async Task ShouldCreateSubActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    
    AddAuthorizationData(PartialSession.Create(user));

    await CreateActivityGroups(user.Id);
    await CreateActivity();

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

  [Fact]
  public async Task ShouldUpdateSubActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    
    AddAuthorizationData(PartialSession.Create(user));

    await CreateActivityGroups(user.Id);
    await CreateActivity();
    await CreateSubActivity();

    var payload = new UpdateSubActivityPayload(
      Title: "Diff Sub Tarefa 1",
      Description: "Diff Descrição da Sub Tarefa 1",
      Checked: true,
      Order: 2
    );
    
    var response = await _httpClient.PutAsync($"/activity-group/{group.Id}/activity/{activity.Id}/subActivity/{subActivity.Id}", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var updated = await _db.SubActivity.FirstOrDefaultAsync(s => s.Id == subActivity.Id);
    
    Assert.NotNull(updated);
    Assert.True(updated.Checked);
    Assert.Equal(payload.Order, updated.Order);
    Assert.Equal(payload.Title, updated.Title);
    Assert.Equal(payload.Description, updated.Description);
  }

  private async Task CreateSubActivity ()
  {
    subActivity = new SubActivityMock()
      .ChangeTitle("Sub Tarefa 1")
      .ChangeDescription("Descrição para Sub Tarefa 1")
      .WithOrder(1)
      .AssignToActivity(activity.Id);

    _db.SubActivity.Add(subActivity);
    await _db.SaveChangesAsync();
  }
  
  private async Task CreateActivity ()
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