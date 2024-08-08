using TrackIt.Tests.Mocks.Entities;

namespace TrackIt.Tests.Unit;

public class SubActivityTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var subActivity = new SubActivityMock();
    
    Assert.False(subActivity.Checked);
    Assert.Equal(0, subActivity.Order);
    Assert.Equal(string.Empty, subActivity.Description);
    Assert.Equal(string.Empty, subActivity.Title);
    Assert.Equal(Guid.Empty, subActivity.ActivityId);
  }
  
  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var activityId = Guid.NewGuid();
    
    var subActivity = new SubActivityMock()
      .ChangeTitle("Sub Tarefa")
      .ChangeDescription("Sub Tarefa de teste")
      .WithOrder(1)
      .AssignToActivity(activityId)
      .WithChecked()
      .ChangeCreatedAt(DateTime.Parse("2024-08-07T00:00:00"));
    
    Assert.True(subActivity.Checked);
    Assert.Equal(1, subActivity.Order);
    Assert.Equal("Sub Tarefa", subActivity.Title);
    Assert.Equal("Sub Tarefa de teste", subActivity.Description);
    Assert.Equal(DateTime.Parse("2024-08-07T00:00:00"), subActivity.CreatedAt);
    Assert.Equal(activityId, subActivity.ActivityId);
  }
}