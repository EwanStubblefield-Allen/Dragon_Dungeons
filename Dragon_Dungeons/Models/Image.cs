using Microsoft.Extensions.Options;

namespace Dragon_Dungeons.Models;

public class Image
{
  public string Public_id { get; set; }
  public string Secure_url { get; set; }
  public string Signature { get; set; }
}

public class CreateImage
{
  public string File { get; set; }
  public string Api_key { get; set; }
  public string Upload_preset { get { return "ml_default"; } }
}

public class RemoveImage
{
  public int IMAGE_API_KEY { get; set; }
  public string Public_id { get; set; }
  public string Signature { get; set; }
  public DateTime Timestamp { get { return DateTime.Now; } }
}