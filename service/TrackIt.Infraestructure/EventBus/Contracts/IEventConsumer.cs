using TrackIt.Entities.Core;
using MassTransit;

namespace TrackIt.Infraestructure.EventBus.Contracts;

public interface IEventConsumer<in TEvent> : IConsumer<TEvent> where TEvent : DomainEvent
{
  new Task Consume (ConsumeContext<TEvent> @event);
}