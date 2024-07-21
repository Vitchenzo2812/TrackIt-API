using TrackIt.Entities.Core;

namespace TrackIt.Entities.Events;

public record SendEmailVerificationEvent (string ValidationObject, string Code) : DomainEvent;