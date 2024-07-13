using TrackIt.Entities.Core;
using TrackIt.Entities;

namespace TrackIt.Tests.Config.Builders;

public class SessionBuilder : Session
{
  public static SessionBuilder Empty ()
  {
    return new SessionBuilder
    {
      Id = Guid.NewGuid(),
      
      Email = "username@gmail.com",
      
      Name = "username",
      
      Hierarchy = Hierarchy.CLIENT,
      
      Income = 2000
    };
  }

  public static SessionBuilder Build (User user)
  {
    return new SessionBuilder
    {
      Id = user.Id,
      
      Email = user.Email?.Value,
      
      Name = user.Name,
      
      Hierarchy = user.Hierarchy,
      
      Income = user.Income
    };
  }
}