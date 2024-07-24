using TrackIt.Entities.Core;
using TrackIt.Entities.Errors;
using TrackIt.Entities.Services;

namespace TrackIt.Entities;

public class User : Aggregate
{
  public string? Name { get; set; }
  
  public Email? Email { get; set; }

  public bool EmailValidated { get; set; }
  
  public Password? Password { get; set; }

  public Hierarchy Hierarchy { get; set; } = Hierarchy.CLIENT;
  
  public double? Income { get; set; }

  public DateTime CreatedAt { get; set; } = DateTimeProvider.Now;
  
  public static User Create (Email email, Password password)
  {
    return new User
    {
      Email = email,
      Password = password
    };
  }
  
  public User SignUp (string email, string password)
  {
    if (Email!.Value == email && !Password.Verify(password, Password!))
      throw new EmailAlreadyInUseError();
      
    if (Email!.Value == email && EmailValidated)
      throw new UserAlreadyExistsError();
    
    return this;
  }

  public void ValidateEmail ()
  {
    EmailValidated = true;
  }
  
  public void Update (string name, double income)
  {
    Name = name;
    Income = income;
  }
}