using TrackIt.Entities;
using TrackIt.Tests.Mocks.Contracts;

namespace TrackIt.Tests.Mocks.Entities;

public class ActivityGroupMock : ActivityGroup, IMock<ActivityGroup>
{
  public ActivityGroupMock ChangeIcon (string icon)
  {
    Icon = icon;

    return this;
  }
  
  public ActivityGroupMock ChangeTitle (string title)
  {
    Title = title;

    return this;
  }
  
  public ActivityGroupMock WithOrder (int order)
  {
    Order = order;

    return this;
  }
  
  public ActivityGroupMock AssignToUser (Guid userId)
  {
    UserId = userId;

    return this;
  }
  
  public ActivityGroupMock InsertActivity (Activity activity)
  {
    Activities.Add(activity);

    return this;
  }
  
  public void Verify (ActivityGroup expect, ActivityGroup current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Icon, current.Icon);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.UserId, current.UserId);

    foreach (var activityExpect in expect.Activities)
    {
      var activityCurrent = current.Activities.Find(a => a.Id == activityExpect.Id);
      
      Assert.NotNull(activityCurrent);
      new ActivityMock().Verify(activityExpect, activityCurrent);
    }
  }
}