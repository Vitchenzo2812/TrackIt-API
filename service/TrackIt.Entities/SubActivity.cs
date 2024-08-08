using TrackIt.Entities.Services;
using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class SubActivity : Entity
{
  public Guid ActivityId { get; set; }
  
  public string Title { get; set; } = string.Empty;
  
  public string Description { get; set; } = string.Empty;
  
  public bool Checked { get; set; }
  
  public int Order { get; set; }
  
  public DateTime CreatedAt = DateTime.Now;
}