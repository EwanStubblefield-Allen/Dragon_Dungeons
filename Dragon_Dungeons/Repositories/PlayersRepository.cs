namespace Dragon_Dungeons.Repositories;

public class PlayersRepository
{
  private readonly IDbConnection _db;

  public PlayersRepository(IDbConnection db)
  {
    _db = db;
  }

  internal List<Player> GetPlayersByCampaignId(string campaignId)
  {
    string sql = "SELECT * FROM players WHERE campaignId = @campaignId;";
    return _db.Query<Player>(sql, new { campaignId }).ToList();
  }

  internal void CreatePlayer(Player playerData)
  {
    string sql = @"
      INSERT INTO players(name, picture, level, class, race, characterId, campaignId)
      VALUES(@Name, @Picture, @Level, @Class, @Race, @CharacterId, @CampaignId);";
    _db.Execute(sql, playerData);
  }
}
