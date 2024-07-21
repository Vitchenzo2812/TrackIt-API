namespace TrackIt.Entities.Errors;

public class TicketCannotBeClosedError : ApplicationError
{
  public TicketCannotBeClosedError () : base(400, "TICKET_CANNOT_BE_CLOSED", "Invalid given code")
  {
  }
}