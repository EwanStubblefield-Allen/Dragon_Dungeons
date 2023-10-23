namespace Dragon_Dungeons.Models;

public class Player : RepoItem
{

  public string Name { get; set; }
  public string Picture { get; set; }
  public string CharacterId { get; set; }
  public string CampaignId { get; set; }
  public int? Level { get; set; }
  public string Class { get; set; }
  public string Race { get; set; }
}