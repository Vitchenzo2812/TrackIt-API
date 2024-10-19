using MediatR;

namespace TrackIt.Entities.Core;

public abstract class Command<TPayload> : IRequest
{
  public TPayload Payload { get; set; }
  
  public Session? Session { get; set; }

  public Command (TPayload payload, Session? session = null)
  {
    Payload = payload;
    Session = session;
  }
}

public abstract class Command<TAggregate, TPayload> : IRequest 
{
  public TAggregate ActivitySubActivityAggregate { get; set; }
  
  public TPayload Payload { get; set; }
  
  public Session? Session { get; set; }

  public Command (TAggregate activitySubActivityAggregate, TPayload payload, Session? session = null)
  {
    ActivitySubActivityAggregate = activitySubActivityAggregate;
    Payload = payload;
    Session = session;
  }
}
public abstract class Command<TAggregate, TPayload, TResult> : IRequest<TResult>
{
  public TAggregate Aggregate { get; set; }
  
  public TPayload Payload { get; set; }
  
  public Session? Session { get; set; }

  public Command (TAggregate aggregate, TPayload payload, Session? session = null)
  {
    Aggregate = aggregate;
    Payload = payload;
    Session = session;
  }
}