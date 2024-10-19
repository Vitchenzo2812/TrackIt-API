using TrackIt.Entities;

namespace TrackIt.Tests.Unit;

public class ActivityGroupTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var group = ActivityGroup.Create();
    
    Assert.Equal(default, group.UserId);
    Assert.Equal(string.Empty, group.Title);
    Assert.Equal(0, group.Order);
  }

  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var userId = Guid.NewGuid();

    var group = ActivityGroup.Create()
      .AssignUser(userId)
      .WithTitle("GROUP_TITLE")
      .WithOrder(1);
    
    Assert.Equal(userId, group.UserId);
    Assert.Equal("GROUP_TITLE", group.Title);
    Assert.Equal(1, group.Order);
  }
}