namespace Dragon_Dungeons.Models
{
  public class Bonus : RepoItem
  {
    public int Str { get; set; }
    public int Dex { get; set; }
    public int Con { get; set; }
    public int Intelligence { get; set; }
    public int Wis { get; set; }
    public int Cha { get; set; }
    public string CharacterId { get; set; }
  }
}