namespace TrackIt.Queries.GetCategories;

public record GetCategoriesResult (
  Guid Id,
  string Title,
  string Description,
  string Icon,
  string IconColor,
  string BackgroundIconColor
)
{
  public static GetCategoriesResult Build (CategoryRow row)
  {
    return new GetCategoriesResult(
      Id: row.Id,
      Title: row.Title,
      Description: row.Description,
      Icon: row.Icon,
      IconColor: row.IconColor,
      BackgroundIconColor: row.BackgroundIconColor
    );
  }
}