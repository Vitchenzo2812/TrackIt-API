namespace TrackIt.Entities.Core;

public class Session : Data
{
  public Guid Id { get; set; }

  public string? Name { get; set; }

  public string? Email { get; set; }

  public Hierarchy Hierarchy { get; set; }

  public double? Income { get; set; }

  public static Session Create (User user)
  {
    return new Session
    {
      Id = user.Id,
      
      Name = user.Name,
      
      Email = user.Email?.Value,
      
      Hierarchy = user.Hierarchy,
      
      Income = user.Income
    };
  }
}