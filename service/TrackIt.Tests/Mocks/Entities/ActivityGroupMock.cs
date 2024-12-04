using TrackIt.Tests.Mocks.Contracts;
using TrackIt.Entities;
using TrackIt.Entities.Activities;
using TrackIt.Queries.GetActivityGroup;

namespace TrackIt.Tests.Mocks.Entities;

public class ActivityGroupMock : ActivityGroup
{
  public static void Verify (GetActivityGroupResult expect, ActivityGroup current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Order, current.Order);
  }
  
  public void Verify (ActivityGroup expect, ActivityGroup current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.UserId, current.UserId);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Order, current.Order);
  }
}