using TrackIt.Commands.ActivityCommands.CreateActivity;
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
}