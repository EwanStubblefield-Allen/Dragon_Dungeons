using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dragon_Dungeons.Services;

public class JsonManager
{
  private static readonly JsonSerializerSettings serializeSettings = new()
  {
    ContractResolver = new LowercaseContractResolver()
  };
  private static readonly JsonSerializerSettings deserializeSettings = new()
  {
    ContractResolver = new UppercaseContractResolver()
  };

  public string SerializeObject(object o)
  {
    return JsonConvert.SerializeObject(o, Formatting.Indented, serializeSettings);
  }

  public Image DeserializeString(string json)
  {
    return JsonConvert.DeserializeObject<Image>(json, deserializeSettings);
  }

  public class LowercaseContractResolver : DefaultContractResolver
  {
    protected override string ResolvePropertyName(string propertyName)
    {
      return propertyName.ToLower();
    }
  }

  public class UppercaseContractResolver : DefaultContractResolver
  {
    protected override string ResolvePropertyName(string propertyName)
    {
      return propertyName.ToUpper();
    }
  }
}