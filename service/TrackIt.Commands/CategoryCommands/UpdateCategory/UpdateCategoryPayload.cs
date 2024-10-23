namespace TrackIt.Commands.CategoryCommands.UpdateCategory;

public record UpdateCategoryPayload(
  string Title,
  string Description,
  string Icon,
  string IconColor,
  string BackgroundIconColor
);