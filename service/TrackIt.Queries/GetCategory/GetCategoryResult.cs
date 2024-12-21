namespace TrackIt.Queries.GetCategory;

public record GetCategoryResult (
  Guid Id,
  string Title,
  string Description,
  string Icon,
  string IconColor,
  string BackgroundIconColor
)
{
  public static GetCategoryResult Build (CategoryRow row)
  {
    return new GetCategoryResult (
      Id: row.Id,
      Title: row.Title,
      Description: row.Description,
      Icon: row.Icon,
      IconColor: row.IconColor,
      BackgroundIconColor: row.BackgroundIconColor
    );
  }
}