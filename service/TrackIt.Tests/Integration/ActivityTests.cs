using TrackIt.Commands.ActivityCommands.CreateActivity;
using TrackIt.Commands.ActivityCommands.UpdateActivity;
using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Mocks.Entities;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration;

public class ActivityTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private ActivityGroupMock group1 { get; set; }
  
  private ActivityGroupMock group2 { get; set; }
  
  private ActivityMock activity1 { get; set; }
  
  private ActivityMock activity2 { get; set; }
  
  [Fact]
  public async Task ShouldCreateActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    
    AddAuthorizationData(PartialSession.Create(user));

    await CreateActivityGroups(user.Id);

    var payload = new CreateActivityPayload(
      Title: "Tarefa 1",
      Description: "Descrição relacionada a tarefa 1"
    );
    
    var response = await _httpClient.PostAsync($"/activity-group/{group1.Id}/activity", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var activityGroup = _db.ActivityGroup
      .Include(aG => aG.Activities)
      .FirstOrDefault(aG => aG.Id == group1.Id);

    Assert.NotNull(activityGroup);
    Assert.Single(activityGroup.Activities);
    
    Assert.Equal(payload.Title, activityGroup.Activities[0].Title);
    Assert.Equal(payload.Description, activityGroup.Activities[0].Description);
    Assert.Equal(1, activityGroup.Activities[0].Order);
  }

  [Fact]
  public async Task ShouldUpdateActivity ()
  {
    var user = await CreateUserWithEmailValidated();

    AddAuthorizationData(PartialSession.Create(user));
    
    await CreateActivityGroups(user.Id);
    await CreateActivity();
    
    var payload = new UpdateActivityPayload(
      Title: "Diff Tarefa 2",
      Description: "Diff Descrição para a tarefa 2",
      Checked: true,
      Order: 1,
      ActivityId: activity2.Id
    ); 
    
    var response = await _httpClient.PutAsync($"/activity-group/{group1.Id}/activity", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var activity = await _db.Activity.FirstOrDefaultAsync(a => a.Id == activity2.Id);
    
    Assert.NotNull(activity);
    Assert.Equal(payload.Order, activity.Order);
    Assert.Equal(payload.Title, activity.Title);
    Assert.Equal(payload.Checked, activity.Checked);
    Assert.Equal(payload.Description, activity.Description);
  }

  private async Task CreateActivity ()
  {
    activity1 = new ActivityMock()
      .ChangeTitle("Tarefa 1")
      .ChangeDescription("Descrição para tarefa 1")
      .WithOrder(1)
      .AssignToGroup(group1.Id);

    activity2 = new ActivityMock()
      .ChangeTitle("Tarefa 2")
      .ChangeDescription("Descrição para tarefa 2")
      .WithOrder(2)
      .AssignToGroup(group2.Id);
    
    _db.Activity.AddRange([activity1, activity2]);

    await _db.SaveChangesAsync();
  }
  
  private async Task CreateActivityGroups (Guid userId)
  {
    group1 = new ActivityGroupMock()
      .ChangeTitle("Grupo de Tarefas 1")
      .ChangeIcon("Icon 1")
      .WithOrder(1)
      .AssignToUser(userId);
    
    group2 = new ActivityGroupMock()
      .ChangeTitle("Grupo de Tarefas 2")
      .ChangeIcon("Icon 2")
      .WithOrder(2)
      .AssignToUser(userId);
    
    _db.ActivityGroup.AddRange([group1, group2]);
    await _db.SaveChangesAsync();
  }
}