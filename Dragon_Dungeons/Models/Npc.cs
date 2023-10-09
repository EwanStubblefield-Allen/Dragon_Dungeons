namespace Dragon_Dungeons.Models;

public class Npc : RepoItem
{
  public string Name { get; set; }
  public string Picture { get; set; }
  public string CharacterId { get; set; }
  public string CampaignId { get; set; }
}