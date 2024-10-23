namespace TrackIt.Commands.CategoryCommands.CreateCategory;

public record CreateCategoryPayload(
  string Title,
  string Description,
  string Icon,
  string IconColor,
  string BackgroundIconColor
);