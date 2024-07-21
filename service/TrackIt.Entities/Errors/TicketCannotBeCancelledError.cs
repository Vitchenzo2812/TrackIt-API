namespace TrackIt.Entities.Errors;

public class TicketCannotBeCancelledError : ApplicationError
{
  public TicketCannotBeCancelledError () : base(400, "TICKET_CANNOT_BE_CANCELLED", "Min time to request other ticket is 2 minutes")
  {
  }
}