using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TrackIt.Entities.Core;
using TrackIt.Entities.Errors;
using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Infraestructure.Security.Errors;

namespace TrackIt.Infraestructure.Security;

public class JwtService : IJwtService
{
  public string GenerateToken (Session session)
  {
    string? secret = Environment.GetEnvironmentVariable("JWT_SECRET");

    if (string.IsNullOrEmpty(secret))
      throw new InternalServerError("Secret not found");

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
      new Claim("id", session.Id.ToString()),
      
      new Claim("name", session.Name),
      
      new Claim("email", session.Email),
      
      new Claim("income", session.Income.ToString())
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      
      Expires = DateTime.UtcNow.AddYears(1),
      
      SigningCredentials = credentials
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  public bool Verify (string token)
  {
    string? secret = Environment.GetEnvironmentVariable("JWT_SECRET");

    if (string.IsNullOrEmpty(secret))
      throw new InternalServerError("Secret not found");

    var tokenValidationParams = new TokenValidationParameters
    {
       ValidateAudience = false,
       
       ValidateIssuer = false,
       
       ValidateIssuerSigningKey = true,
       
       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
       
       ValidateLifetime = false
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    try
    {
      tokenHandler.ValidateToken(token, tokenValidationParams, out var securityToken);

      if (
        securityToken is not JwtSecurityToken jwtSecurityToken ||
        
        !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
      )
      {
        return false;
      }

      return true;
    }
    catch (Exception)
    {
      return false;
    }
  }

  public Session Decode (string token)
  {
    string? secret = Environment.GetEnvironmentVariable("JWT_SECRET");

    if (string.IsNullOrEmpty(secret))
      throw new InternalServerError("Secret not found");
    
    var tokenValidationParams = new TokenValidationParameters
    {
      ValidateAudience = false,
      
      ValidateIssuer = false,
      
      ValidateIssuerSigningKey = true,
      
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
      
      ValidateLifetime = false
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    tokenHandler.ValidateToken(token, tokenValidationParams, out var securityToken);
    
    if (
      securityToken is not JwtSecurityToken jwtSecurityToken ||

      !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
    )
    {
      throw new InvalidTokenError();
    }

    var jwtToken = tokenHandler.ReadJwtToken(token);
    return CreateSessionByToken(jwtToken);
  }

  private static Session CreateSessionByToken (JwtSecurityToken? jwtToken)
  {
    var id = jwtToken.Claims.First(c => c.Type == "id").Value;
    var name = jwtToken.Claims.First(c => c.Type == "name").Value;
    var email = jwtToken.Claims.First(c => c.Type == "email").Value;
    var income = jwtToken.Claims.First(c => c.Type == "income").Value;

    double.TryParse(income, out var incomeConverted);
    
    return new Session
    {
      Id = new Guid(id),
      
      Name = name,
      
      Email = email,
      
      Income = incomeConverted
    };
  }
}