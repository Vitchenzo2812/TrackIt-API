using TrackIt.Commands.ActivityCommands.CreateActivity;
using TrackIt.Commands.ActivityCommands.UpdateActivity;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config.Builders;
using TrackIt.Tests.Config;
using TrackIt.Entities;
using System.Net;

namespace TrackIt.Tests.Integration.Activities;

public class ActivityTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private ActivityGroup activityGroup1 { get; set; }
  private ActivityGroup activityGroup2 { get; set; }
  private ActivityGroup activityGroup3 { get; set; }
  private Activity activity1 { get; set; }
  private Activity activity2 { get; set; }
  
  [Fact]
  public async Task ShouldCreateActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateGroups(user.Id);
    
    var payload = new CreateActivityPayload(
      Title: "ACTIVY_1",
      Description: "SOME_DESCRIPTION_ACTIVITY_1",
      Priority: ActivityPriority.MEDIUM,
      Order: 1
    );
    
    var response = await _httpClient.PostAsync($"/group/{activityGroup1.Id}/activity", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var created = await _db.Activities
      .Where(x => x.ActivityGroupId == activityGroup1.Id)
      .ToListAsync();

    Assert.Single(created);
    Assert.Equal(created[0].Title, payload.Title);
    Assert.Equal(created[0].Description, payload.Description);
    Assert.Equal(created[0].Priority, payload.Priority);
    Assert.Equal(created[0].Order, payload.Order);
  }

  [Fact]
  public async Task ShouldUpdateActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateActivities(user.Id);

    var payload = new UpdateActivityPayload(
      Title: "DIFF_ACTIVITY_1",
      Description: "DIFF_ACTIVITY_1_DESCRIPTION",
      Priority: ActivityPriority.HIGH,
      Order: 2
    );
    
    var response = await _httpClient.PutAsync($"/group/{activityGroup2.Id}/activity/{activity1.Id}", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var updated = await _db.Activities.FirstOrDefaultAsync(x => x.Id == activity1.Id);
    
    Assert.NotNull(updated);
    Assert.NotNull(updated.Description);
    Assert.Equal(updated.Title, payload.Title);
    Assert.Equal(updated.Description, payload.Description);
    Assert.Equal(updated.Priority, payload.Priority);
    Assert.Equal(updated.Order, payload.Order);
  }

  [Fact]
  public async Task ShouldThrowActivityNotFoundUpdate ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateGroups(user.Id);
    
    var payload = new UpdateActivityPayload(
      Title: "DIFF_ACTIVITY_1",
      Description: "DIFF_ACTIVITY_1_DESCRIPTION",
      Priority: ActivityPriority.HIGH,
      Order: 2
    );
    
    var response = await _httpClient.PutAsync($"/group/{activityGroup2.Id}/activity/{Guid.NewGuid()}", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Activity not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }

  [Fact]
  public async Task ShouldThrowActivityGroupNotFoundError ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));
    
    var payload = new CreateActivityPayload(
      Title: "ACTIVY_1",
      Description: "SOME_DESCRIPTION_ACTIVITY_1",
      Priority: ActivityPriority.MEDIUM,
      Order: 1
    );
    
    var response = await _httpClient.PostAsync($"/group/{Guid.NewGuid()}/activity", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Activity Group not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  #region setup for tests
  
    private async Task CreateGroups (Guid userId)
    {
      activityGroup1 = ActivityGroup.Create()
        .WithTitle("GROUP_1")
        .WithOrder(1)
        .AssignUser(userId);
      
      activityGroup2 = ActivityGroup.Create()
        .WithTitle("GROUP_2")
        .WithOrder(2)
        .AssignUser(userId);

      var diffUser = await CreateUserWithEmailValidated();
      
      activityGroup3 = ActivityGroup.Create()
        .WithTitle("GROUP_1_DIFF_USER")
        .WithOrder(1)
        .AssignUser(diffUser.Id);
      
      _db.ActivityGroups.AddRange([activityGroup1, activityGroup2]);
      await _db.SaveChangesAsync();
    }

    private async Task CreateActivities (Guid userId)
  {
    await CreateGroups(userId);
    
    activity1 = Activity.Create()
      .WithTitle("ACTIVITY_1")
      .WithDescription("ACTIVITY_1_DESCRIPTION")
      .WithPriority(ActivityPriority.MEDIUM)
      .WithOrder(2)
      .AssignToGroup(activityGroup2.Id);
    
    activity2 = Activity.Create()
      .WithTitle("ACTIVITY_2")
      .WithPriority(ActivityPriority.HIGH)
      .WithOrder(1)
      .AssignToGroup(activityGroup2.Id);

    _db.Activities.AddRange([activity1, activity2]);
    await _db.SaveChangesAsync();
  }
  
  #endregion
}