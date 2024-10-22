using System.ComponentModel;

namespace TrackIt.Infraestructure.Extensions;

public static class EnumExtension
{
  public static string Description<TEnum>(this TEnum value)
  {
    DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false);
    return customAttributes.Length != 0 ? customAttributes[0].Description : string.Empty;
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