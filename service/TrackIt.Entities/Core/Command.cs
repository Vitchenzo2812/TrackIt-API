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

public abstract class Command<TAggregateId, TPayload> : IRequest 
{
  public TAggregateId AggregateId { get; set; }
  
  public TPayload Payload { get; set; }
  
  public Session? Session { get; set; }

  public Command (TAggregateId aggregateId, TPayload payload, Session? session = null)
  {
    AggregateId = aggregateId;
    Payload = payload;
    Session = session;
  }
}
public abstract class Command<TAggregateId, TPayload, TResult> : IRequest<TResult>
{
  public TAggregateId AggregateId { get; set; }
  
  public TPayload Payload { get; set; }
  
  public Session? Session { get; set; }

  public Command (TAggregateId aggregateId, TPayload payload, Session? session = null)
  {
    AggregateId = aggregateId;
    Payload = payload;
    Session = session;
  }
}