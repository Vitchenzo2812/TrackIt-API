using System.ComponentModel;

namespace TrackIt.Entities.Core;

public enum TicketSituation
{
  [Description("OPEN")]
  OPEN,
  
  [Description("CANCELLED")]
  CANCELLED,
  
  [Description("CLOSED")]
  CLOSED
}