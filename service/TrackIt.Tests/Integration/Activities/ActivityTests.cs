using TrackIt.Commands.ActivityCommands.CreateActivity;
using TrackIt.Commands.ActivityCommands.UpdateActivity;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using TrackIt.Queries.Views.HomePage;
using Microsoft.EntityFrameworkCore;
using TrackIt.Queries.GetActivities;
using TrackIt.Tests.Config.Builders;
using TrackIt.Entities.Activities;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Activities;

public class ActivityTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private ActivityGroup activityGroup1 { get; set; }
  private ActivityGroup activityGroup2 { get; set; }
  private ActivityGroup activityGroup3 { get; set; }
  private Activity activity1 { get; set; }
  private Activity activity2 { get; set; }
  private SubActivity subActivity1 { get; set; }

  [Fact]
  public async Task ShouldGetHomePageInfo ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));
    
    await CreateActivities(user.Id);
    
    var response = await _httpClient.GetAsync($"/group/home-page?activityGroupId={activityGroup2.Id}&completedActivitiesPerPage=2&incompletedActivitiesPerPage=2");
    var result = await response.ToData<HomePageView>();

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    
    Assert.Equal(50, result.PercentageCompletedActivities);

    Assert.Equal(1, result.CompletedActivities.Page);
    Assert.Equal(1, result.CompletedActivities.Pages);
    
    foreach (var completedActivity in result.CompletedActivities.Data)
    {
      Assert.Equal(activity1.Title, completedActivity.Title);
      Assert.Equal(activity1.Description,completedActivity.Description);
    }
    
    Assert.Equal(1, result.IncompleteActivities.Page);
    Assert.Equal(1, result.IncompleteActivities.Pages);

    foreach (var incompletedActivity in result.IncompleteActivities.Data)
    {
      Assert.Equal(activity2.Title, incompletedActivity.Title);
      Assert.Equal(activity2.Description,incompletedActivity.Description);
    }
  }

  [Fact]
  public async Task ShouldGetActivitiesByGroupId ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));
    
    await CreateActivities(user.Id);

    var response = await _httpClient.GetAsync($"/group/{activityGroup2.Id}/activity");
    var result = await response.ToData<List<GetActivitiesResult>>();
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    Assert.Equal(2, result.Count);
    
    Assert.Equal(activity2.Id, result[0].Id);
    Assert.Equal(activity1.Id, result[1].Id);
    
    List<Activity> activities = [activity1, activity2];

    foreach (var resultActivity in result)
    {
      var activity = activities.Find(x => x.Id == resultActivity.Id);
      
      Assert.NotNull(activity);
      Assert.Equal(resultActivity.Id, activity.Id);
      Assert.Equal(resultActivity.Title, activity.Title);
      Assert.Equal(resultActivity.Description, activity.Description);
      Assert.Equal(resultActivity.Order, activity.Order);
      Assert.Equal(resultActivity.Checked, activity.Checked);
      Assert.Equal(resultActivity.Priority, activity.Priority);
      Assert.Equal(resultActivity.CompletedAt.HasValue, activity.CompletedAt.HasValue);
    }
  }
  
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
      Order: 2,
      IsChecked: true
    );
    
    var response = await _httpClient.PutAsync($"/group/{activityGroup2.Id}/activity/{activity1.Id}", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var updated = await _db.Activities.FirstOrDefaultAsync(x => x.Id == activity1.Id);
    
    Assert.NotNull(updated);
    Assert.True(updated.Checked);
    Assert.NotNull(updated.Description);
    Assert.Equal(updated.Title, payload.Title);
    Assert.Equal(updated.Description, payload.Description);
    Assert.Equal(updated.Priority, payload.Priority);
    Assert.Equal(updated.Order, payload.Order);
  }

  [Fact]
  public async Task ShouldDeleteActivity ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateActivities(user.Id);
    
    var response = await _httpClient.DeleteAsync($"/group/{activityGroup2.Id}/activity/{activity2.Id}");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var deletedActivity = await _db.Activities.FirstOrDefaultAsync(x => x.Id == activity2.Id);
    var subActivities = await _db.SubActivities.ToListAsync();
    
    Assert.Null(deletedActivity);
    Assert.Empty(subActivities);
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
      Order: 2,
      IsChecked: false
    );
    
    var response = await _httpClient.PutAsync($"/group/{activityGroup2.Id}/activity/{Guid.NewGuid()}", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Activity not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldThrowActivityNotFoundDelete ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateGroups(user.Id);
    
    var response = await _httpClient.DeleteAsync($"/group/{activityGroup2.Id}/activity/{Guid.NewGuid()}");
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Activity not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldThrowForbiddenIfActivityDoesntBelongToTheGroup ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateActivities(user.Id);

    var payload = new UpdateActivityPayload(
      Title: "DIFF_ACTIVITY_1",
      Description: "DIFF_ACTIVITY_1_DESCRIPTION",
      Priority: ActivityPriority.HIGH,
      Order: 2,
      IsChecked: true
    );
    
    var response = await _httpClient.PutAsync($"/group/{activityGroup3.Id}/activity/{activity1.Id}", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Activity doesn't belong to this activity group", result.Message);
    Assert.Equal("FORBIDDEN_ERROR", result.Code);
    Assert.Equal(403, result.StatusCode);
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
      
      activityGroup3 = ActivityGroup.Create()
        .WithTitle("GROUP_3")
        .WithOrder(3)
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
        .ShouldCheck(true)
        .AssignToGroup(activityGroup2.Id);
      
      activity2 = Activity.Create()
        .WithTitle("ACTIVITY_2")
        .WithPriority(ActivityPriority.HIGH)
        .WithOrder(1)
        .ShouldCheck(false)
        .AssignToGroup(activityGroup2.Id);

      subActivity1 = SubActivity.Create()
        .WithTitle("SUB_ACTIVITY_1")
        .WithPriority(ActivityPriority.LOW)
        .WithOrder(1)
        .AssignToActivity(activity2.Id);
      
      _db.Activities.AddRange([activity1, activity2]);
      _db.SubActivities.Add(subActivity1);
      await _db.SaveChangesAsync();
    }
  
  #endregion
}