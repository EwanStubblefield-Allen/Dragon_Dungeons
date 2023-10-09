namespace Dragon_Dungeons.Repositories;

public class BonusesRepository
{
  private readonly IDbConnection _db;

  public BonusesRepository(IDbConnection db)
  {
    _db = db;
  }

  internal void CreateBonus(Bonus bonusData)
  {
    string sql = @"
      INSERT INTO bonuses(id, str, dex, con, intelligence, wis, cha, characterId)
      VALUES(@Id, @Str, @Dex, @Con, @Intelligence, @Wis, @Cha, @CharacterId);";
    _db.Execute(sql, bonusData);
  }
}