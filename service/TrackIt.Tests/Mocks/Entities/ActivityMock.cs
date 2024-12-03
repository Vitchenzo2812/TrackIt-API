using TrackIt.Tests.Mocks.Contracts;
using TrackIt.Entities;
using TrackIt.Entities.Activities;
using TrackIt.Queries.GetActivity;

namespace TrackIt.Tests.Mocks.Entities;

public class ActivityMock : Activity
{
  public static void Verify (GetActivityResult expect, Activity current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Order, current.Order);
    Assert.Equal(expect.Checked, current.Checked);
    Assert.Equal(expect.Priority, current.Priority);
    Assert.Equal(expect.CompletedAt, current.CompletedAt);
    Assert.Equal(expect.Description, current.Description);
    Assert.Equal(expect.ActivityGroupId, current.ActivityGroupId);
  }
  
  public static void Verify (Activity expect, Activity current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Order, current.Order);
    Assert.Equal(expect.Checked, current.Checked);
    Assert.Equal(expect.Priority, current.Priority);
    Assert.Equal(expect.CreatedAt, current.CreatedAt);
    Assert.Equal(expect.CompletedAt, current.CompletedAt);
    Assert.Equal(expect.Description, current.Description);
    Assert.Equal(expect.ActivityGroupId, current.ActivityGroupId);
  }
}