namespace Dragon_Dungeons.Repositories;

public class PlayersRepository(IDbConnection db)
{
  private readonly IDbConnection _db = db;

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

  internal List<Campaign> GetPlayersByUserId(string userId)
  {
    string sql = @"
      SELECT
        p.*,
        c.*
      FROM
        players p
        JOIN campaigns c ON c.id = p.campaignId
      WHERE
        p.creatorId = @userId;";
    return _db.Query<Player, Campaign, Campaign>(
      sql,
      (player, campaigns) =>
      {
        campaigns.Players = new List<Player> { player };
        return campaigns;
      },
      new { userId }).ToList();
  }

  internal void CreatePlayerByCampaignId(Player playerData)
  {
    string sql = @"
      INSERT INTO players(id, name, picture, class, race, creatorId, characterId, campaignId)
      VALUES(@Id, @Name, @Picture, @Class, @Race, @CreatorId, @CharacterId, @CampaignId);";
    _db.Execute(sql, playerData);
  }

  internal void RemovePlayerByCampaignId(string playerId)
  {
    string sql = "DELETE FROM players WHERE id = @playerId LIMIT 1;";
    _db.Execute(sql, new { playerId });
  }
}