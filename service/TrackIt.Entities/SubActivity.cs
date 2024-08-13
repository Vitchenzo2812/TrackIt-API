using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class SubActivity : Entity
{
  public Guid ActivityId { get; set; }
  
  public string Title { get; set; } = string.Empty;
  
  public string? Description { get; set; }
  
  public bool Checked { get; set; }
  
  public int Order { get; set; }
  
  public DateTime CreatedAt = DateTime.Now;

  public static SubActivity Create (
    string title,
    string? description,
    bool isChecked,
    int order,
    Guid activityId
  )
  {
    return new SubActivity
    {
      Title = title,
      Description = description,
      Checked = isChecked,
      Order = order,
      ActivityId = activityId,
    };
  }
}