using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Dragon_Dungeons.Services;

public class ImagesService(JsonManager jsonManager, Config config)
{
  private readonly JsonManager _jsonManager = jsonManager;
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

  internal Image ImageRequest(object imageData, string type)
  {
    HttpClient client = new()
    {
      BaseAddress = new Uri($"https://api.cloudinary.com/v1_1/dtcatqouc/image")
    };
    string json = _jsonManager.SerializeObject(imageData);
    StringContent req = new(json, Encoding.UTF8, "application/json");
    HttpResponseMessage res = client.PostAsync(type, req).Result;
    if (res.IsSuccessStatusCode)
    {
      string resContent = res.Content.ReadAsStringAsync().Result;

      return _jsonManager.DeserializeString(resContent);
    }
    else
    {
      throw new Exception(res.ReasonPhrase);
    }
  }
}
