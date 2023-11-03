namespace Dragon_Dungeons.Models;

public class Campaign : RepoItem
{
  public string Name { get; set; }
  public string Description { get; set; }
  public string PrivateNotes { get; set; }
  public string PublicNotes { get; set; }
  public string Events { get; set; }
  public List<Npc> Npcs { get; set; }
  public string Monsters { get; set; }
  public List<Player> Players { get; set; }
  public List<Comment> Comments { get; set; }
  public string CreatorId { get; set; }
  public Account Creator { get; set; }
}