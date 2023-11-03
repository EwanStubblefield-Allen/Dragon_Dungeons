namespace Dragon_Dungeons.Models;

public class Comment : RepoItem
{
  public string Description { get; set; }
  public string CampaignId { get; set; }
  public string CreatorId { get; set; }
  public Profile Creator { get; set; }
}
