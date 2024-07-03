using System.ComponentModel.DataAnnotations;

namespace TrackIt.Entities.Core;

public class Entity
{
  [Key] public Guid Id { get; set; } = Guid.NewGuid();
}