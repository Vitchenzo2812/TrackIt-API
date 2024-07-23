namespace TrackIt.Commands.Auth.EmailValidation;

public record EmailValidationPayload (
  Guid UserId,
  
  string Code
);