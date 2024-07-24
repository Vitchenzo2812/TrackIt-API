using TrackIt.Entities;

namespace TrackIt.Queries.Views;

public record UserResourceView (
  Guid Id,

  string? Name,

  string? Email,
  
  Hierarchy Hierarchy,
  
  DateTime CreatedAt
)
{
  public static UserResourceView Build (User user)
  {
    return new UserResourceView(
      Id: user.Id,
      Name: user.Name,
      Email: user.Email?.Value,
      Hierarchy: user.Hierarchy,
      CreatedAt: user.CreatedAt
    );
  }
}