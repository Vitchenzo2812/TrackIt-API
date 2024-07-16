using TrackIt.Entities.Core;
using TrackIt.Entities.Events;

namespace TrackIt.Entities;

public class User : Aggregate
{
  public string? Name { get; set; }
  
  public Email? Email { get; set; }
  
  public Password? Password { get; set; }

  public Hierarchy Hierarchy { get; set; } = Hierarchy.CLIENT;
  
  public double? Income { get; set; }

  public static User Create (Email email, Password password)
  {
    return new User().InternalCreate(email, password);
  }

  private User InternalCreate (Email email, Password password)
  {
    Email = email;
    Password = password;

    Commit(new SignUpEvent(Id));
    
    return this;
  }

  public void Update (string name, double income)
  {
    Name = name;
    Income = income;
  }
}