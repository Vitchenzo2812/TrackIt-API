using System.ComponentModel.DataAnnotations;

namespace TrackIt.Infraestructure.Security.Models;

public class RefreshToken
{
  [Key] public Guid UserId { get; set; } = Guid.NewGuid();
  
  public string Token { get; set; }

  public static RefreshToken Create (Guid userId, string token)
  {
    return new RefreshToken
    {
      UserId = userId,
      
      Token = token
    };
  }
}