using TrackIt.Tests.Mocks.Contracts;
using TrackIt.Entities;

namespace TrackIt.Tests.Mocks.Entities;

public class ActivityMock : Activity, IMock<Activity>
{
  public ActivityMock ChangeTitle (string title)
  {
    Title = title;

    return this;
  }
  
  public ActivityMock ChangeDescription (string description)
  {
    Description = description;

    return this;
  }
  
  public ActivityMock WithOrder (int order)
  {
    Order = order;

    return this;
  }
  
  public ActivityMock AssignToGroup (Guid groupId)
  {
    ActivityGroupId = groupId;
    
    return this;
  }

  public ActivityMock InsertSubActivity (SubActivity subActivity)
  {
    SubActivities.Add(subActivity);

    return this;
  }
  
  public ActivityMock WithChecked ()
  {
    Checked = true;

    return this;
  }
  
  public ActivityMock ChangeCreatedAt (DateTime date)
  {
    CreatedAt = date;

    return this;
  }
  
  public void Verify (Activity expect, Activity current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Checked, current.Checked);
    Assert.Equal(expect.Description, current.Description);
    Assert.Equal(expect.ActivityGroupId, current.ActivityGroupId);
    Assert.Equal(expect.CreatedAt, current.CreatedAt);
    Assert.Equal(expect.Order, current.Order);

    foreach (var subExpect in expect.SubActivities)
    {
      var subCurrent = current.SubActivities.Find(s => s.Id == subExpect.Id);
      
      Assert.NotNull(subCurrent);
      new SubActivityMock().Verify(subExpect, subCurrent);
    }
  }
}