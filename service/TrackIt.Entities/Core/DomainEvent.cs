namespace TrackIt.Entities.Core;

public record DomainEvent
{
  public DateTime CreatedAt { get; set; } = DateTime.Now;
}