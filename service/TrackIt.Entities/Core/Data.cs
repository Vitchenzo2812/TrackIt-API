using System.Reflection;
using TrackIt.Entities.Errors;

namespace TrackIt.Entities.Core;

public class Data
{
  public string[] Keys
  {
    get
    {
      PropertyInfo[] props = GetType().GetProperties();
      string[] keys = new string[props.Length];

      for (int i = 0; i < props.Length; i++)
        keys[i] = props[i].Name;

      return keys;
    }
  }

  public object GetValueFromKey (string key)
  {
    PropertyInfo? prop = GetType().GetProperty(key);

    if (prop != null && prop.GetValue(this) != null)
    {
      return prop.GetValue(this);
    }

    throw new InternalServerError($"Property '{key}' doesn't exists");
  }
}