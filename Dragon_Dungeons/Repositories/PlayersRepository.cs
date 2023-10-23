namespace Dragon_Dungeons.Repositories;

public class PlayersRepository
{
  private readonly IDbConnection _db;

  public PlayersRepository(IDbConnection db)
  {
    _db = db;
  }

  internal Player GetPlayerById(string playerId)
  {
    string sql = "SELECT * FROM players WHERE id = @playerId LIMIT 1;";
    return _db.QueryFirstOrDefault<Player>(sql, new { playerId });
  }

  internal List<Player> GetPlayersByCampaignId(string campaignId)
  {
    string sql = "SELECT * FROM players WHERE campaignId = @campaignId;";
    return _db.Query<Player>(sql, new { campaignId }).ToList();
  }

  internal string CreatePlayer(Player playerData)
  {
    string sql = @"
      INSERT INTO players(name, picture, level, class, race, creatorId, characterId, campaignId)
      VALUES(@Name, @Picture, @Level, @Class, @Race, @CreatorId, @CharacterId, @CampaignId);
      SELECT LAST_INSERT_ID();";
    return _db.ExecuteScalar<string>(sql, playerData);
  }
}
