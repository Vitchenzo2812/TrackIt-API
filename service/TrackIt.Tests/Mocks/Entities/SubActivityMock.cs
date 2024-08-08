using TrackIt.Entities;
using TrackIt.Tests.Mocks.Contracts;

namespace TrackIt.Tests.Mocks.Entities;

public class SubActivityMock : SubActivity, IMock<SubActivity>
{
  public SubActivityMock ChangeTitle (string title)
  {
    Title = title;

    return this;
  }
  
  public SubActivityMock ChangeDescription (string description)
  {
    Description = description;

    return this;
  }
  
  public SubActivityMock WithOrder (int order)
  {
    Order = order;

    return this;
  }
  
  public SubActivityMock AssignToActivity (Guid activityId)
  {
    ActivityId = activityId;
    
    return this;
  }
  
  public SubActivityMock WithChecked ()
  {
    Checked = true;

    return this;
  }
  
  public SubActivityMock ChangeCreatedAt (DateTime date)
  {
    CreatedAt = date;

    return this;
  }
  
  public void Verify (SubActivity expect, SubActivity current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Order, current.Order);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Checked, current.Checked);
    Assert.Equal(expect.CreatedAt, current.CreatedAt);
    Assert.Equal(expect.ActivityId, current.ActivityId);
    Assert.Equal(expect.Description, current.Description);
  }
}