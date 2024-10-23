using TrackIt.Entities.Core;

namespace TrackIt.Entities.Events;

public record DeleteCategoryEvent (Guid CategoryId) : DomainEvent;