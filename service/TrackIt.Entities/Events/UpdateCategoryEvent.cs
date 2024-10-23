using TrackIt.Entities.Core;

namespace TrackIt.Entities.Events;

public record UpdateCategoryEvent (Guid CategoryId, string Icon, string IconColor, string BackgroundIconColor) : DomainEvent;