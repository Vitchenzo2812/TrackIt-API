using Microsoft.EntityFrameworkCore.Diagnostics;
using TrackIt.Entities.Core;
using System.Reflection;
using Newtonsoft.Json;
using MassTransit;

namespace TrackIt.Infraestructure.Database.Interceptor;

public class PublishEvents : SaveChangesInterceptor
{
  private readonly IBus _bus;

  public PublishEvents (IBus bus) => _bus = bus;
    
  public override async ValueTask<int> SavedChangesAsync (
    SaveChangesCompletedEventData eventData, 
    int result,
    CancellationToken cancellationToken = new CancellationToken()
  )
  {
    var context = eventData.Context;

    if (context is not null)
    {
      var domainEvents = context.ChangeTracker
        .Entries<Aggregate>()
        .SelectMany(e =>
        {
          var domainEvents = e.Entity.DomainEvents;
          
          e.Entity.Push();

          return domainEvents;
        })
        .ToList();
      
      foreach (var domainEvent in domainEvents)
        await PublishEvent(domainEvent);
    }
    
    return await base.SavedChangesAsync(eventData, result, cancellationToken);
  }

  private async Task PublishEvent (DomainEvent domainEvent)
  {
    var assembly = domainEvent.GetType().Assembly.FullName;
    var type = domainEvent.GetType().FullName;
    var content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

    if (assembly is null || type is null)
      return;
        
    var domainEventType = Assembly.Load(assembly).GetType(type);

    if (domainEventType is null)
      return;
    
    var realEvent = JsonConvert.DeserializeObject(content, domainEventType);

    if (realEvent is null) 
      return;
        
    await _bus.Publish(realEvent, domainEventType);
  }
}