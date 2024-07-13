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

  public static T IntFromDescription<T> (this string description) where T : Enum
  {
    var type = typeof(T);

    foreach (var field in type.GetFields())
    {
      if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
      {
        if (attr.Description == description)
          return (T)field.GetValue(null);
      }
    }

    throw new ArgumentException($"Nenhum valor de enum corresponde à descrição '{description}'", nameof(description));
  }
}