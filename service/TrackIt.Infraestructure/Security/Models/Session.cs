using TrackIt.Entities;

namespace TrackIt.Infraestructure.Security.Models;

public class Session
{
  public Guid Id { get; set; }
  
  public string? Name { get; set; }
  
  public string? Email { get; set; }
  
  public Hierarchy Hierarchy { get; set; }
  
  public double? Income { get; set; }
  
  public required string Token { get; set; }
  
  public required string RefreshToken { get; set; }

  public static Session Create (User user, string token, string refreshToken)
  {
    return new Session
    {
      Id = user.Id,
      
      Name = user.Name,
      
      Email = user.Email?.Value,
      
      Hierarchy = user.Hierarchy,
      
      Income = user.Income,
      
      Token = token,
      
      RefreshToken = refreshToken
    };
  }
}