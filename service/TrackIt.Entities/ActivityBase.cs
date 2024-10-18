using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class ActivityBase<TEntity> : Aggregate where TEntity : ActivityBase<TEntity>
{
  public required string Title { get; set; }
  public required string? Description { get; set; }
  public required ActivityPriority Priority { get; set; }
  public required int Order { get; set; }
  public required bool Checked { get; set; }
  public required DateTime? CompletedAt { get; set; }
  public readonly DateTime CreatedAt = DateTime.Now;
  
  public TEntity WithTitle (string title)
  {
    Title = title;
    return (TEntity)this;
  }

  public TEntity WithDescription (string? description)
  {
    Description = description;
    return (TEntity)this;
  }

  public TEntity WithPriority (ActivityPriority priority)
  {
    Priority = priority;
    return (TEntity)this;
  }

  public TEntity WithOrder (int order)
  {
    Order = order;
    return (TEntity)this;
  }

  public TEntity ShouldCheck (bool check)
  {
    if (check) 
      CompletedAt = DateTime.Now;
    
    Checked = check;
    return (TEntity)this;
  }
}