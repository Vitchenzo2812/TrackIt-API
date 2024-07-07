using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace TrackIt.Infraestructure.Extensions;

public static class ObjectExtension
{
  public static StringContent ToJson (this object data)
  {
    return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
  }

  public static async Task<T> ToData<T> (this HttpResponseMessage response, NamingStrategy? strategy = null)
  {
    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
    {
      ContractResolver = new DefaultContractResolver
      {
        NamingStrategy = strategy
      },
      Formatting = Formatting.Indented
    })!;
  }
}