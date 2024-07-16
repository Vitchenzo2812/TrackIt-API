using TrackIt.Entities.Core;

namespace TrackIt.Entities.Events;

public record SignUpEvent (Guid UserId) : DomainEvent;