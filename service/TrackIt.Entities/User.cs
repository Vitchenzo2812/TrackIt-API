using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class User : Entity
{
  public string Name { get; set; }
  
  public Email Email { get; set; }
  
  public Password Password { get; set; }
  
  public double Income { get; set; }
}