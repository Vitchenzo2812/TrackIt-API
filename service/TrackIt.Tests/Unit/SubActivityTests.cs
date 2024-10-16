using TrackIt.Entities;

namespace TrackIt.Tests.Unit;

public class SubActivityTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var subActivity = SubActivity.Create();
    
    Assert.False(subActivity.Checked);
    Assert.Null(subActivity.Description);
    Assert.Null(subActivity.CompletedAt);
    Assert.Equal(0, subActivity.Order);
    Assert.Equal(string.Empty, subActivity.Title);
    Assert.Equal(default, subActivity.ActivityId);
    Assert.Equal(ActivityPriority.LOW, subActivity.Priority);
  }

  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var activityId = Guid.NewGuid();
    
    var subActivity = SubActivity.Create()
      .AssignToActivity(activityId)
      .WithTitle("SUB_ACTIVITY_TITLE")
      .WithDescription("SUB_ACTIVITY_DESCRIPTION")
      .WithPriority(ActivityPriority.HIGH)
      .WithOrder(1)
      .ShouldCheck(true);
    
    Assert.True(subActivity.Checked);
    Assert.NotNull(subActivity.CompletedAt);
    Assert.Equal(1, subActivity.Order);
    Assert.Equal(activityId, subActivity.ActivityId);
    Assert.Equal("SUB_ACTIVITY_TITLE", subActivity.Title);
    Assert.Equal("SUB_ACTIVITY_DESCRIPTION", subActivity.Description);
    Assert.Equal(ActivityPriority.HIGH, subActivity.Priority);
  }
}