using MediatR;

namespace TrackIt.Entities.Core;

public abstract class Query<TParams, TResult> : IRequest<TResult>
{
  public TParams Params { get; set; }
  
  public Session? Session { get; set; }

  public Query (TParams @params, Session? session = null)
  {
    Params = @params;
    Session = session;
  }
}