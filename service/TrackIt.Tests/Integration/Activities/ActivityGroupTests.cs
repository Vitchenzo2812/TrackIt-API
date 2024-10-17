using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config.Builders;
using TrackIt.Tests.Config;
using TrackIt.Entities;
using System.Net;
using TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;
using TrackIt.Tests.Mocks.Entities;

namespace TrackIt.Tests.Integration.Activities;

public class ActivityGroupTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private ActivityGroup activityGroup1 { get; set; }
  private ActivityGroup activityGroup2 { get; set; }
  
  [Fact]
  public async Task ShouldCreateActivityGroup ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    const string titleActivityGroup = "GROUP_1";
    
    var payload = new CreateActivityGroupPayload(titleActivityGroup);
    var response = await _httpClient.PostAsync("/activity/group", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var created = await _db.ActivityGroups.ToListAsync();

    Assert.Single(created);
    Assert.Equal(titleActivityGroup, created[0].Title);
    Assert.Equal(1, created[0].Order);
  }

  [Fact]
  public async Task ShouldUpdateActivityGroup ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateActivityGroups(user);

    var payload = new UpdateActivityGroupPayload("DIFF_GROUP_2", 3);
    
    var response = await _httpClient.PutAsync($"activity/group/{activityGroup2.Id}", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var updated = await _db.ActivityGroups.FirstOrDefaultAsync(x => x.Id == activityGroup2.Id);
    
    Assert.NotNull(updated);
    Assert.Equal(activityGroup2.Id, updated.Id);
    Assert.Equal("DIFF_GROUP_2", updated.Title);
    Assert.Equal(3, updated.Order);
  }

  [Fact]
  public async Task ShouldDeleteActivityGroup ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateActivityGroups(user);

    var response = await _httpClient.DeleteAsync($"/activity/group/{activityGroup1.Id}");
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var existingGroups = await _db.ActivityGroups.ToListAsync();

    Assert.Single(existingGroups);
    Assert.Equal(existingGroups[0].Id, activityGroup2.Id);

    var deletedActivities = await _db.Activities.ToListAsync();
    var deletedSubActivities = await _db.SubActivities.ToListAsync();
    
    Assert.Empty(deletedActivities);
    Assert.Empty(deletedSubActivities);
  }
  
  private async Task CreateActivityGroups (UserMock user)
  {
    activityGroup1 = ActivityGroup.Create()
      .AssignUser(user.Id)
      .WithTitle("GROUP_1")
      .WithOrder(1);
    
    activityGroup2 = ActivityGroup.Create()
      .AssignUser(user.Id)
      .WithTitle("GROUP_2")
      .WithOrder(2);

    var activity1 = Activity.Create()
      .WithTitle("ACTIVITY_1")
      .WithOrder(2)
      .AssignToGroup(activityGroup1.Id);
    
    var activity2 = Activity.Create()
      .WithTitle("ACTIVITY_2")
      .WithOrder(1)
      .AssignToGroup(activityGroup1.Id);

    var subActivity1 = SubActivity.Create()
      .WithTitle("SUB_ACTIVITY_1")
      .WithOrder(1)
      .AssignToActivity(activity1.Id);
    
    _db.ActivityGroups.AddRange([activityGroup1, activityGroup2]);
    _db.Activities.AddRange([activity1, activity2]);
    _db.SubActivities.Add(subActivity1);
    
    await _db.SaveChangesAsync();
  }
}