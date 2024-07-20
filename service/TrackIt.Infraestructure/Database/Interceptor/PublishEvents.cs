using Microsoft.EntityFrameworkCore.Diagnostics;
using TrackIt.Entities.Core;
using MassTransit;

namespace TrackIt.Infraestructure.Database.Interceptor;

public class PublishEvents : SaveChangesInterceptor
{
  private readonly IBus _bus;

  public PublishEvents (IBus bus)
  {
    _bus = bus;
  }
    
  public override async ValueTask<int> SavedChangesAsync (
    SaveChangesCompletedEventData eventData, 
    int result,
    CancellationToken cancellationToken = new CancellationToken()
  )
  {
    var context = eventData.Context;

    var domainEvents = context.ChangeTracker
      .Entries<Aggregate>()
      .SelectMany(e => e.Entity.DomainEvents)
      .ToList();

    foreach (var domainEvent in domainEvents)
      await _bus.Publish(domainEvent, cancellationToken);

    foreach (var aggregate in context.ChangeTracker.Entries<Aggregate>())
      aggregate.Entity.Push();
    
    return await base.SavedChangesAsync(eventData, result, cancellationToken);
  }
}