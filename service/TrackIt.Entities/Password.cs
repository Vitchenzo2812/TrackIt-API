using System.Security.Cryptography;
using System.Text;
using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class Password : Entity
{
  public string Hash { get; set; }
  
  public string Salt { get; set; }

  private Password (string hash, string salt)
  {
    Hash = hash;
    Salt = salt;
  }
  
  public static Password Create (string password)
  {
    var salt = GenerateSalt();
    var hash = ComputeHash(password, salt);

    return new Password(hash, salt);
  }

  public static bool Verify (string plainText, Password password)
  {
    var hash = ComputeHash(plainText, password.Salt);
    return hash == password.Hash;
  }

  private static string GenerateSalt ()
  {
    var buffer = new byte[16];
    RandomNumberGenerator.Fill(buffer);
    return Convert.ToBase64String(buffer);
  }

  private static string ComputeHash (string password, string salt)
  {
    using (var sha256 = SHA256.Create())
    {
      var saltedPassword = password + salt;
      var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
      var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
      return Convert.ToBase64String(hashBytes);
    }
  }
}