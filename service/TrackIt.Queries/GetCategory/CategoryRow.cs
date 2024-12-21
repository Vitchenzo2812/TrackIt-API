namespace TrackIt.Queries.GetCategory;

public record CategoryRow (
  Guid Id,
  string Title,
  string Description,
  string Icon,
  string IconColor,
  string BackgroundIconColor
);