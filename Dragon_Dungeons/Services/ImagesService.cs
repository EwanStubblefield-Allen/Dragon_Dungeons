using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Dragon_Dungeons.Services;

public class ImagesService(JsonManager jsonManager, Config config)
{
  private readonly JsonManager _jsonManager = jsonManager;
  private readonly Config _config = config;

  internal string ConvertToSha256(RemoveImage removeImage)
  {
    removeImage.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    string message = $"public_id={removeImage.Public_Id}&timestamp={removeImage.Timestamp}{_config.IMAGE_API_SECRET}";
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

  internal string GenerateImage(string prompt)
  {
    HttpClient client = new()
    {
      BaseAddress = new Uri("https://api.openai.com/v1/"),
    };
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.OPENAI_API_KEY);
    StringContent req = new(prompt, Encoding.UTF8, "application/json");
    HttpResponseMessage res = client.PostAsync("images/generations", req).Result;
    client.Dispose();
    if (res.IsSuccessStatusCode)
    {
      return res.Content.ReadAsStringAsync().Result;
    }
    else
    {
      throw new Exception(res.ReasonPhrase);
    }
  }

  internal string ImageRequest(object imageData, string type)
  {
    HttpClient client = new()
    {
      BaseAddress = new Uri("https://api.cloudinary.com/v1_1/dtcatqouc/image/")
    };
    string json = _jsonManager.SerializeObject(imageData);
    StringContent req = new(json, Encoding.UTF8, "application/json");
    HttpResponseMessage res = client.PostAsync(type, req).Result;
    client.Dispose();
    if (res.IsSuccessStatusCode)
    {
      return res.Content.ReadAsStringAsync().Result;
    }
    else
    {
      throw new Exception(res.ReasonPhrase);
    }
  }
}