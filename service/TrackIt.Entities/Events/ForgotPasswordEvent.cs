using TrackIt.Entities.Core;

namespace TrackIt.Entities.Events;

public record ForgotPasswordEvent (string ValidationObject, string Code) : DomainEvent;