using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class Activity : Aggregate
{
  public string Title { get; set; } = string.Empty;
  
  public string? Description { get; set; }
  
  public bool Checked { get; set; }

  public int Order { get; set; }

  public Guid ActivityGroupId { get; set; }
  
  public List<SubActivity> SubActivities { get; set; } = [];
  
  public DateTime CreatedAt = DateTime.Now;

  public static Activity Create (
    Guid activityGroupId,
    string title, 
    int order,
    string? description
  )
  {
    return new Activity
    {
      Title = title,
      
      Order = order,
      
      Description = description,
      
      ActivityGroupId = activityGroupId
    };
  }

  public void Update (
    string title,
    string? description,
    int order,
    bool isChecked
  )
  {
    Title = title;
    Description = description;
    Order = order;
    Checked = isChecked;
  }
}