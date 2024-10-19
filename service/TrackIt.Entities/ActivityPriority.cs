using System.ComponentModel;

namespace TrackIt.Entities;

public enum ActivityPriority
{
  [Description("LOW")]
  LOW,
  
  [Description("MEDIUM")]
  MEDIUM,
  
  [Description("HIGH")]
  HIGH
}