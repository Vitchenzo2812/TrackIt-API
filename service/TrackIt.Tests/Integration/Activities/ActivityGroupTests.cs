using TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Queries.GetActivityGroups;
using TrackIt.Infraestructure.Web.Dto;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config.Builders;
using TrackIt.Tests.Mocks.Entities;
using TrackIt.Entities.Activities;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Activities;

public class ActivityGroupTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private ActivityGroup activityGroup1 { get; set; }
  private ActivityGroup activityGroup2 { get; set; }
  private ActivityGroup activityGroup3 { get; set; }

  [Fact]
  public async Task ShouldGetActivityGroups ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateActivityGroups(user);

    var response = await _httpClient.GetAsync("/group");
    var result = await response.ToData<List<GetActivityGroupsResult>>();
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    Assert.Equal(2, result.Count);
    
    List<ActivityGroup> groups = [activityGroup1, activityGroup2];

    foreach (var resultGroup in result)
    {
      var group = groups.Find(x => x.Id == resultGroup.Id);
      
      Assert.NotNull(group);
      Assert.Equal(resultGroup.Id, group.Id);
      Assert.Equal(resultGroup.Title, group.Title);
      Assert.Equal(resultGroup.Order, group.Order);
    }
  }
  
  [Fact]
  public async Task ShouldCreateActivityGroup ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    const string titleActivityGroup = "GROUP_1";
    
    var payload = new CreateActivityGroupPayload(titleActivityGroup);
    var response = await _httpClient.PostAsync("/group", payload.ToJson());
    
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
    
    var response = await _httpClient.PutAsync($"group/{activityGroup2.Id}", payload.ToJson());
    
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

    var response = await _httpClient.DeleteAsync($"/group/{activityGroup1.Id}");
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var existingGroups = await _db.ActivityGroups
      .Where(x => x.UserId == user.Id)
      .ToListAsync();

    Assert.Single(existingGroups);
    Assert.Equal(existingGroups[0].Id, activityGroup2.Id);

    var deletedActivities = await _db.Activities.ToListAsync();
    var deletedSubActivities = await _db.SubActivities.ToListAsync();
    
    Assert.Empty(deletedActivities);
    Assert.Empty(deletedSubActivities);
  }
  
  [Fact]
  public async Task ShouldThrowActivityGroupNotFoundUpdate ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    var payload = new UpdateActivityGroupPayload("DIFF_GROUP_2", 3);
    
    var response = await _httpClient.PutAsync($"/group/{Guid.NewGuid()}", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Activity Group not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldThrowActivityGroupNotFoundDelete ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    var response = await _httpClient.DeleteAsync($"/group/{Guid.NewGuid()}");
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Activity Group not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldThrowForbiddenIfGroupDoesntBelongToTheUser ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateActivityGroups(user);

    var payload = new UpdateActivityGroupPayload("DIFF_GROUP_2", 3);
    
    var response = await _httpClient.PutAsync($"group/{activityGroup3.Id}", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Forbidden Error", result.Message);
    Assert.Equal("FORBIDDEN_ERROR", result.Code);
    Assert.Equal(403, result.StatusCode);
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

    var diffUser = await CreateUserWithEmailValidated();
    
    activityGroup3 = ActivityGroup.Create()
      .AssignUser(diffUser.Id)
      .WithTitle("GROUP_3")
      .WithOrder(1);

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
    
    _db.ActivityGroups.AddRange([activityGroup1, activityGroup2, activityGroup3]);
    _db.Activities.AddRange([activity1, activity2]);
    _db.SubActivities.Add(subActivity1);
    
    await _db.SaveChangesAsync();
  }
}