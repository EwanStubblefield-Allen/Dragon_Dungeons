namespace Dragon_Dungeons.Models
{
  public class Profile : RepoItem
  {
    public string Name { get; set; }
    public string Picture { get; set; }
    public string CoverImg { get; set; }
  }

  public class Account : Profile
  {
    public string Email { get; set; }
  }
}