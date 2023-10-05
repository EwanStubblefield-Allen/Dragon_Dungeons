namespace Dragon_Dungeons.Models
{
  public class Character : RepoItem
  {
    public string Name { get; set; }
    public string Picture { get; set; }
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int TempHp { get; set; }
    public int Speed { get; set; }
    public int HitDie { get; set; }
    public int Level { get; set; }
    public string Class { get; set; }
    public string Race { get; set; }
    public string Alignment { get; set; }
    public int Age { get; set; }
    public int Feet { get; set; }
    public int Inches { get; set; }
    public int Weight { get; set; }
    public string Eyes { get; set; }
    public string Skin { get; set; }
    public string Hair { get; set; }
    public string Features { get; set; }
    public string Background { get; set; }
    public string Backstory { get; set; }
    public string PersonalityTraits { get; set; }
    public string Ideals { get; set; }
    public string Bonds { get; set; }
    public string Flaws { get; set; }
    public bool Manual { get; set; }
    public int Str { get; set; }
    public int Dex { get; set; }
    public int Con { get; set; }
    public int Intelligence { get; set; }
    public int Wis { get; set; }
    public int Cha { get; set; }
    public int Bonus { get; set; }
    public string Skills { get; set; }
    public string Proficiencies { get; set; }
    public string Cantrips { get; set; }
    public string Spells { get; set; }
    public string Equipment { get; set; }
    public string CreatorId { get; set; }
    public Profile Creator { get; set; }
  }
}