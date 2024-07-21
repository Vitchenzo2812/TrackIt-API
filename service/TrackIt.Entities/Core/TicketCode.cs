using TrackIt.Entities.Services;

namespace TrackIt.Entities.Core;

public class TicketCode : Entity
{
  public string Value { get; set; }

  public TicketCode ()
  {
    Value = GenerateTicketCode.Code;
  }

  public static TicketCode Generate ()
  {
    return new TicketCode();
  }
}