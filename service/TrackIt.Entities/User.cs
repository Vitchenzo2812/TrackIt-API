using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class User : Aggregate
{
  public string? Name { get; set; }
  
  public Email Email { get; set; }
  
  public Password Password { get; set; }
  
  public double? Income { get; set; }

  public static User Create (Email email, Password password)
  {
    return new User
    {
      Email = email,
      Password = password
    };
  }
}