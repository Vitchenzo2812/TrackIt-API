using TrackIt.Tests.Mocks.Contracts;
using TrackIt.Entities;
using TrackIt.Entities.Activities;

namespace TrackIt.Tests.Mocks.Entities;

public class ActivityGroupMock : ActivityGroup, IMock<ActivityGroup>
{
  public void Verify (ActivityGroup expect, ActivityGroup current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.UserId, current.UserId);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Order, current.Order);
  }
}