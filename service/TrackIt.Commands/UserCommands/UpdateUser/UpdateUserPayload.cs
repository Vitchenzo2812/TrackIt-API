namespace TrackIt.Commands.UserCommands.UpdateUser;

public record UpdateUserPayload (
  string Name,
  
  double Income
);