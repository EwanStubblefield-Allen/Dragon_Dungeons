using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dragon_Dungeons.Services;

public class ImagesService(Config config)
{
  private readonly Config _config = config;

  internal string ConvertToSha256(RemoveImage removeImage)
  {
    string message = $"public_id={removeImage.Public_id}&timestamp={removeImage.Timestamp}{_config.IMAGE_API_SECRET}";
    byte[] msgBuffer = Encoding.UTF8.GetBytes(message);

    // Hash the message
    byte[] hashBuffer = SHA256.HashData(msgBuffer);

    // Convert byte array to hex string
    StringBuilder builder = new();
    foreach (byte b in hashBuffer)
    {
      builder.Append(b.ToString("x2"));
    }
    return builder.ToString();
  }

  internal object ImageRequest(object imageData, string type)
  {
    HttpClient client = new()
    {
      BaseAddress = new Uri($"https://api.cloudinary.com/v1_1/dtcatqouc/image")
    };
    string json = JsonManager.SerializeObject(imageData);
    StringContent req = new(json, Encoding.UTF8, "application/json");
    HttpResponseMessage res = client.PostAsync(type, req).Result;
    if (res.IsSuccessStatusCode)
    {
      string resContent = res.Content.ReadAsStringAsync().Result;

      return JsonManager.DeserializeString(resContent);
    }
    else
    {
      throw new Exception(res.ReasonPhrase);
    }
  }

  internal Image OpenAiRequest()
  {
    return new();
  }
}

internal class JsonManager
{
  private static readonly JsonSerializerSettings serializeSettings = new()
  {
    ContractResolver = new LowercaseContractResolver()
  };
  private static readonly JsonSerializerSettings deserializeSettings = new()
  {
    ContractResolver = new UppercaseContractResolver()
  };

  public static string SerializeObject(object o)
  {
    return JsonConvert.SerializeObject(o, Formatting.Indented, serializeSettings);
  }

  public static object DeserializeString(string json)
  {
    return JsonConvert.DeserializeObject(json);
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
