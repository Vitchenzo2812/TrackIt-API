using TrackIt.Commands.SubActivityCommands.CreateSubActivity;
using TrackIt.Commands.SubActivityCommands.UpdateSubActivity;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config.Builders;
using TrackIt.Entities.Activities;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Activities;

public class SubActivityTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private ActivityGroup activityGroup1 { get; set; }
  private ActivityGroup activityGroup2 { get; set; }
  private ActivityGroup activityGroup3 { get; set; }
  private Activity activity1 { get; set; }
  private Activity activity2 { get; set; }
  private SubActivity subActivity1 { get; set; }
  private SubActivity subActivity2 { get; set; }
  private SubActivity subActivity3 { get; set; }
  
  [Fact]
  public async Task ShouldCreateSubActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateSubActivities(user.Id);

    var payload = new CreateSubActivityPayload(
      Title: "SUB_ACTIVITY_4",
      Description: "SUB_ACTIVITY_4_DESCRIPTION",
      Priority: ActivityPriority.MEDIUM,
      Order: 2
    );
    
    var response = await _httpClient.PostAsync($"group/{activityGroup2.Id}/activity/{activity2.Id}/sub", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var allSubActivities = await _db.SubActivities.ToListAsync();
    
    Assert.Equal(4, allSubActivities.Count);

    List<SubActivity> subActivities = [subActivity1, subActivity2, subActivity3];

    foreach (var subActivity in allSubActivities)
    {
      var isntNewSubActivity = subActivities.Find(x => x.Id == subActivity.Id);

      if (isntNewSubActivity is not null)
        continue;
      
      Assert.NotNull(subActivity.Description);
      Assert.Equal(subActivity.Title, payload.Title);
      Assert.Equal(subActivity.Description, payload.Description);
      Assert.Equal(subActivity.Priority, payload.Priority);
      Assert.Equal(subActivity.Order, payload.Order);
    }
  }

  [Fact]
  public async Task ShouldUpdateSubActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateSubActivities(user.Id);

    var payload = new UpdateSubActivityPayload(
      Title: "DIFF_SUB_ACTIVITY_2",
      Description: null,
      Priority: ActivityPriority.MEDIUM,
      Order: 2,
      IsChecked: false
    );
    
    var response = await _httpClient.PutAsync($"/group/{activityGroup2.Id}/activity/{activity1.Id}/sub/{subActivity2.Id}", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var updated = await _db.SubActivities.FirstOrDefaultAsync(x => x.Id == subActivity2.Id);
    
    Assert.NotNull(updated);
    Assert.False(updated.Checked);
    Assert.Null(updated.Description);
    Assert.Equal(updated.Title, payload.Title);
    Assert.Equal(updated.Order, payload.Order);
    Assert.Equal(updated.Priority, payload.Priority);
  }

  [Fact]
  public async Task ShouldDeleteSubActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateSubActivities(user.Id);

    var response = await _httpClient.DeleteAsync($"/group/{activityGroup2.Id}/activity/{activity2.Id}/sub/{subActivity3.Id}");
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var deleted = await _db.SubActivities.FirstOrDefaultAsync(x => x.Id == subActivity3.Id);
    
    Assert.Null(deleted);
  }
  
  [Fact]
  public async Task ShouldThrowSubActivityNotFoundUpdate ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));
    
    await CreateSubActivities(user.Id);

    var payload = new UpdateSubActivityPayload(
      Title: "DIFF_SUB_ACTIVITY_2",
      Description: null,
      Priority: ActivityPriority.MEDIUM,
      Order: 2,
      IsChecked: false
    );
    
    var response = await _httpClient.PutAsync($"/group/{activityGroup2.Id}/activity/{activity1.Id}/sub/{Guid.NewGuid()}", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("SubActivity not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldThrowSubActivityNotFoundDelete ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));
    
    await CreateSubActivities(user.Id);
    
    var response = await _httpClient.DeleteAsync($"/group/{activityGroup2.Id}/activity/{activity1.Id}/sub/{Guid.NewGuid()}");
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("SubActivity not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldThrowForbiddenIfSubActivityDoesntBelongToTheActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateSubActivities(user.Id);

    var payload = new UpdateSubActivityPayload(
      Title: "DIFF_SUB_ACTIVITY_3",
      Description: null,
      Priority: ActivityPriority.MEDIUM,
      Order: 2,
      IsChecked: false
    );
    
    var response = await _httpClient.PutAsync($"/group/{activityGroup2.Id}/activity/{activity1.Id}/sub/{subActivity3.Id}", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("SubActivity doesn't belong to this activity", result.Message);
    Assert.Equal("FORBIDDEN_ERROR", result.Code);
    Assert.Equal(403, result.StatusCode);
  }

  
  [Fact]
  public async Task ShouldThrowActivityNotFound ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateGroups(user.Id);
    
    await CreateSubActivities(user.Id);

    var payload = new UpdateSubActivityPayload(
      Title: "DIFF_SUB_ACTIVITY_2",
      Description: null,
      Priority: ActivityPriority.MEDIUM,
      Order: 2,
      IsChecked: false
    );
    
    var response = await _httpClient.PutAsync($"/group/{activityGroup2.Id}/activity/{Guid.NewGuid()}/sub/{subActivity2.Id}", payload.ToJson());
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
    
    await CreateSubActivities(user.Id);

    var payload = new UpdateSubActivityPayload(
      Title: "DIFF_SUB_ACTIVITY_2",
      Description: null,
      Priority: ActivityPriority.MEDIUM,
      Order: 2,
      IsChecked: false
    );
    
    var response = await _httpClient.PutAsync($"/group/{Guid.NewGuid()}/activity/{activity1.Id}/sub/{subActivity2.Id}", payload.ToJson());
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
      
      activityGroup3 = ActivityGroup.Create()
        .WithTitle("GROUP_1_DIFF_USER")
        .WithOrder(1)
        .AssignUser(userId);
        
      _db.ActivityGroups.AddRange([activityGroup1, activityGroup2, activityGroup3]);
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

    private async Task CreateSubActivities (Guid userId)
    {
      await CreateActivities(userId);

      subActivity1 = SubActivity.Create()
        .WithTitle("SUB_ACTIVITY_1")
        .WithDescription("SUB_ACTIVITY_1_DESCRIPTION")
        .WithPriority(ActivityPriority.MEDIUM)
        .WithOrder(1)
        .ShouldCheck(true)
        .AssignToActivity(activity1.Id);
      
      subActivity2 = SubActivity.Create()
        .WithTitle("SUB_ACTIVITY_2")
        .WithPriority(ActivityPriority.LOW)
        .WithOrder(2)
        .ShouldCheck(true)
        .AssignToActivity(activity1.Id);
      
      subActivity3 = SubActivity.Create()
        .WithTitle("SUB_ACTIVITY_3")
        .WithDescription("SUB_ACTIVITY_3_DESCRIPTION")
        .WithPriority(ActivityPriority.HIGH)
        .WithOrder(1)
        .AssignToActivity(activity2.Id);
      
      _db.SubActivities.AddRange([subActivity1, subActivity2, subActivity3]);
      await _db.SaveChangesAsync();
    }
    
  #endregion
}