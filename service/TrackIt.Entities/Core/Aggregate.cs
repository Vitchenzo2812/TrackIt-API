using System.ComponentModel.DataAnnotations.Schema;

namespace TrackIt.Entities.Core;

public class Aggregate : Entity
{
  [NotMapped] public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

  public void Commit (DomainEvent domainEvent)
  {
    DomainEvents.Add(domainEvent);
  }

  public void Push ()
  {
    DomainEvents = new List<DomainEvent>();
  }
}