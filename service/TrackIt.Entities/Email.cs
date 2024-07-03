using System.Text.RegularExpressions;
using TrackIt.Entities.Errors;

namespace TrackIt.Entities;

public class Email
{
  private string _value = string.Empty;

  public string Value
  {
    get => _value;

    private set
    {
      if (!IsValid(value))
        throw new InvalidEmailError();
      
      _value = value;
    }
  }

  protected Email (string value)
  {
    ArgumentException.ThrowIfNullOrEmpty(value);

    Value = value;
  }

  public static Email FromAddress (string address)
  {
    return new Email(address);
  }

  protected bool IsValid (string value)
  {
    string pattern = @"^[\w+-\.]+@([\w-]+\.)+[\w-]{2,4}$";

    return new Regex(pattern).IsMatch(value);
  }
}