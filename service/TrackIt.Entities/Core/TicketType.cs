using System.ComponentModel;

namespace TrackIt.Entities.Core;

public enum TicketType
{
  [Description("EMAIL_VERIFICATION")]
  EMAIL_VERIFICATION,
  
  [Description("RESET_PASSWORD")]
  RESET_PASSWORD
}