using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TrackIt.Entities.Core;
using TrackIt.Entities.Errors;

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
    var hash = ComputeHash(IsStrongPassword(password), salt);

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

  private static string IsStrongPassword (string password)
  {
    HasMinCharacter(password);
    return password;
  }
  
  private static void HasMinCharacter (string password)
  {
    if (password.Length < 8)
      throw new InvalidPasswordError();

    HasUpperCaseLetter(password);
  }

  private static void HasUpperCaseLetter (string password)
  {
    if (!Regex.IsMatch(password, @"[A-Z]"))
      throw new InvalidPasswordError();
    
    HasLowerCaseLetter(password);
  }

  private static void HasLowerCaseLetter (string password)
  {
    if (!Regex.IsMatch(password, @"[a-z]"))
      throw new InvalidPasswordError();

    HasDigit(password);
  }

  private static void HasDigit (string password)
  {
    if (!Regex.IsMatch(password, @"[0-9]"))
      throw new InvalidPasswordError();

    HasSpecialCharacter(password);
  }

  private static void HasSpecialCharacter (string password)
  {
    if (!Regex.IsMatch(password, @"[\W_]"))
      throw new InvalidPasswordError();
  }
}