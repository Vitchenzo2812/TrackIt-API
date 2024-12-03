using TrackIt.Queries.GetSubActivities;
using TrackIt.Entities.Activities;

namespace TrackIt.Tests.Mocks.Entities;

public class SubActivityMock : SubActivity
{
  public static void Verify (SubActivity expect, GetSubActivitiesResult current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Order, current.Order);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Checked, current.Checked);
    Assert.Equal(expect.Priority, current.Priority);
    Assert.Equal(expect.ActivityId, current.ActivityId);
    Assert.Equal(expect.Description, current.Description);
  }
  
  public static void Verify (SubActivity expect, SubActivity current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Order, current.Order);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Checked, current.Checked);
    Assert.Equal(expect.Priority, current.Priority);
    Assert.Equal(expect.CreatedAt, current.CreatedAt);
    Assert.Equal(expect.ActivityId, current.ActivityId);
    Assert.Equal(expect.Description, current.Description);
    Assert.Equal(expect.CompletedAt, current.CompletedAt);
  }
}