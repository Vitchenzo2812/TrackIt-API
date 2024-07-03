using System.ComponentModel;

namespace TrackIt.Infraestructure.Extensions;

public static class EnumExtension
{
  public static string Description<TEnum> (this TEnum value)
  {
    DescriptionAttribute[] attributes = (DescriptionAttribute[])value!
      .GetType()
      .GetField(value.ToString())
      .GetCustomAttributes(typeof(DescriptionAttribute), false);

    return attributes.Length > 0 ? attributes[0].Description : string.Empty;
  }
}