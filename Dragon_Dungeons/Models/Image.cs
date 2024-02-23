namespace Dragon_Dungeons.Models;

public class GeneratedImage
{
  public string Data { get; set; }
}

public class CreateImage
{
  public string File { get; set; }
  public string Api_key { get; set; }
  public string Upload_preset { get { return "ml_default"; } }
}

public class RemoveImage
{
  public string Api_Key { get; set; }
  public string Public_Id { get; set; }
  public string Signature { get; set; }
  public long Timestamp { get; set; }
}