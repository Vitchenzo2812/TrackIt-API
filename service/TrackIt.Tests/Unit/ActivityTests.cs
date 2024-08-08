using TrackIt.Tests.Mocks.Entities;

namespace TrackIt.Tests.Unit;

public class ActivityTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var activity = new ActivityMock();
    
    Assert.False(activity.Checked);
    Assert.Null(activity.Description);
    Assert.Empty(activity.SubActivities);
    Assert.Equal(0, activity.Order);
    Assert.Equal(string.Empty, activity.Title);
    Assert.Equal(Guid.Empty, activity.ActivityGroupId);
  }
  
  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var activityGroupId = Guid.NewGuid();
    
    var activity = new ActivityMock()
      .ChangeTitle("Tarefa")
      .ChangeDescription("Tarefa de teste")
      .WithOrder(1)
      .AssignToGroup(activityGroupId)
      .ChangeCreatedAt(DateTime.Parse("2024-08-07T00:00:00"))
      .WithChecked();

    var subActivity = new SubActivityMock()
      .ChangeTitle("Sub Tarefa")
      .ChangeDescription("Sub Tarefa de teste")
      .AssignToActivity(activity.Id)
      .ChangeCreatedAt(DateTime.Parse("2024-08-08T00:00:00"))
      .WithOrder(0);

    activity.InsertSubActivity(subActivity);
    
    Assert.True(activity.Checked);
    Assert.Equal(1, activity.Order);
    Assert.Equal("Tarefa", activity.Title);
    Assert.Equal("Tarefa de teste", activity.Description);
    Assert.Equal(DateTime.Parse("2024-08-07T00:00:00"), activity.CreatedAt);
    Assert.Equal(activityGroupId, activity.ActivityGroupId);
    Assert.Single(activity.SubActivities);
    
    Assert.False(activity.SubActivities[0].Checked);
    Assert.Equal(0, activity.SubActivities[0].Order);
    Assert.Equal(activity.Id, activity.SubActivities[0].ActivityId);
    Assert.Equal("Sub Tarefa", activity.SubActivities[0].Title);
    Assert.Equal("Sub Tarefa de teste", activity.SubActivities[0].Description);
    Assert.Equal(DateTime.Parse("2024-08-08T00:00:00"), activity.SubActivities[0].CreatedAt);
  }
}