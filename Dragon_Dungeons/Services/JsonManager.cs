using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dragon_Dungeons.Services;

public class JsonManager
{
  private static readonly JsonSerializerSettings serializeSettings = new()
  {
    ContractResolver = new LowercaseContractResolver()
  };

  public string SerializeObject(object o)
  {
    return JsonConvert.SerializeObject(o, Formatting.Indented, serializeSettings);
  }

  public class LowercaseContractResolver : DefaultContractResolver
  {
    protected override string ResolvePropertyName(string propertyName)
    {
      return propertyName.ToLower();
    }
  }
}