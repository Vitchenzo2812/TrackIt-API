using TrackIt.Entities.Errors;
using TrackIt.Entities.Events;
using TrackIt.Entities.Services;

namespace TrackIt.Entities.Core;

public class Ticket : Aggregate
{
  public Guid UserId { get; set; }

  public TicketCode Code { get; set; } = TicketCode.Generate();
  
  public string ValidationObject { get; set; }
  
  public TicketType Type { get; set; }

  public TicketSituation Situation { get; set; } = TicketSituation.OPEN;
  
  public DateTime CreatedAt { get; set; } = DateTimeProvider.Now;

  public static Ticket Create (Guid userId, TicketType type, string validationObject)
  {
    return new Ticket
    {
      UserId = userId,
      
      Type = type,
      
      ValidationObject = validationObject
    };
  }
  
  public void Cancel ()
  {
    const int MIN_MINUTES_TO_CANCEL = 2;

    if ((DateTimeProvider.Now - CreatedAt).TotalMinutes <= MIN_MINUTES_TO_CANCEL)
      throw new TicketCannotBeCancelledError();

    Situation = TicketSituation.CANCELLED;
  }

  public bool CanReSend ()
  {
    const int MIN_SECONDS_TO_RE_SEND = 45;

    return (DateTimeProvider.Now - CreatedAt).TotalSeconds > MIN_SECONDS_TO_RE_SEND;
  }
  
  public void Close (string givenCode)
  {
    if (Code.Value != givenCode)
      throw new TicketCannotBeClosedError();

    Situation = TicketSituation.CLOSED;
  }

  public void SendEmailVerification ()
  {
    Commit(new SendEmailVerificationEvent(ValidationObject, Code.Value));
  }
}