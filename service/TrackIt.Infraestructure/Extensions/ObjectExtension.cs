using Newtonsoft.Json;
using System.Text;

namespace TrackIt.Infraestructure.Extensions;

public static class ObjectExtension
{
  public static StringContent ToJson (this object data)
  {
    return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
  }

  public static async Task<T> ToData<T> (this HttpResponseMessage response)
  {
    var result = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(result)!;
  }
}