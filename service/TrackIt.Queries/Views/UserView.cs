using TrackIt.Entities;

namespace TrackIt.Queries.Views;

public record UserView (
  Guid Id,

  string? Name,

  string? Email,

  string? PasswordMask,
  
  Hierarchy Hierarchy,

  double? Income
)
{
  public static UserView Build (User user)
  {
    return new UserView(
      Id: user.Id,
      Name: user.Name,
      Email: user.Email?.Value,
      PasswordMask: user.Password?.MaskPassword(),
      Hierarchy: user.Hierarchy,
      Income: user.Income
    );
  }
}