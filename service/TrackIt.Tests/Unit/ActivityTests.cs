using TrackIt.Entities;

namespace TrackIt.Tests.Unit;

public class ActivityTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var activity = Activity.Create();
    
    Assert.False(activity.Checked);
    Assert.Null(activity.Description);
    Assert.Null(activity.CompletedAt);
    Assert.Equal(0, activity.Order);
    Assert.Equal(string.Empty, activity.Title);
    Assert.Equal(default, activity.ActivityGroupId);
    Assert.Equal(ActivityPriority.LOW, activity.Priority);
  }

  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var activityGroupId = Guid.NewGuid();
    
    var activity = Activity.Create()
      .AssignToGroup(activityGroupId)
      .WithTitle("ACTIVITY_TITLE")
      .WithDescription("ACTIVITY_DESCRIPTION")
      .WithPriority(ActivityPriority.MEDIUM)
      .WithOrder(1)
      .ShouldCheck(true);
    
    Assert.True(activity.Checked);
    Assert.NotNull(activity.CompletedAt);
    Assert.Equal(1, activity.Order);
    Assert.Equal(activityGroupId, activity.ActivityGroupId);
    Assert.Equal("ACTIVITY_TITLE", activity.Title);
    Assert.Equal("ACTIVITY_DESCRIPTION", activity.Description);
    Assert.Equal(ActivityPriority.MEDIUM, activity.Priority);
  }
}