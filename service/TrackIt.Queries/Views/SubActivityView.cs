using TrackIt.Entities;

namespace TrackIt.Queries.Views;

public record SubActivityView (
  Guid Id,
  
  string Title,
  
  string? Description,
  
  bool Checked,
  
  int Order,
  
  DateTime CreatedAt
)
{
  public static SubActivityView Build (SubActivity subActivity)
  {
    return new SubActivityView(
      Id: subActivity.Id,
      Title: subActivity.Title,
      Description: subActivity.Description,
      Checked: subActivity.Checked,
      Order: subActivity.Order,
      CreatedAt: subActivity.CreatedAt
    );
  }
}