using System.ComponentModel;

namespace TrackIt.Entities.Activities;

public enum ActivityPriority
{
  [Description("LOW")]
  LOW,
  
  [Description("MEDIUM")]
  MEDIUM,
  
  [Description("HIGH")]
  HIGH
}