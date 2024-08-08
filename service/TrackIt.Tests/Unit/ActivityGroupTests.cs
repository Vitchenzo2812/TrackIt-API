using TrackIt.Tests.Mocks.Entities;

namespace TrackIt.Tests.Unit;

public class ActivityGroupTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var group = new ActivityGroupMock();
    
    Assert.Equal(0, group.Order);
    Assert.Equal(string.Empty, group.Title);
    Assert.Equal(string.Empty, group.Icon);
    Assert.Equal(Guid.Empty, group.UserId);
    Assert.Empty(group.Activities);
  }
  
  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var userId = Guid.NewGuid();
    
    var group = new ActivityGroupMock()
      .ChangeTitle("Grupo de Tarefa")
      .ChangeIcon("icone")
      .AssignToUser(userId)
      .WithOrder(1);
    
    var activity = new ActivityMock()
      .ChangeTitle("Tarefa")
      .ChangeDescription("Tarefa de teste")
      .WithOrder(0)
      .AssignToGroup(group.Id)
      .WithChecked()
      .ChangeCreatedAt(DateTime.Parse("2024-08-07T00:00:00"));

    group.InsertActivity(activity);
    
    Assert.Equal("Grupo de Tarefa", group.Title);
    Assert.Equal("icone", group.Icon);
    Assert.Equal(1, group.Order);
    Assert.Equal(userId, group.UserId);
    
    Assert.Equal("Tarefa", group.Activities[0].Title);
    Assert.Equal("Tarefa de teste", group.Activities[0].Description);
    Assert.Equal(group.Id, group.Activities[0].ActivityGroupId);
    Assert.True(group.Activities[0].Checked);
    Assert.Equal(0, group.Activities[0].Order);
    Assert.Equal(DateTime.Parse("2024-08-07T00:00:00"), group.Activities[0].CreatedAt);
    Assert.Empty(group.Activities[0].SubActivities);
  }
}