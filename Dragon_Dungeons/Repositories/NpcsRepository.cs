namespace Dragon_Dungeons.Repositories;

public class NpcsRepository
{
  private readonly IDbConnection _db;

  public NpcsRepository(IDbConnection db)
  {
    _db = db;
  }

  internal Npc GetNpcById(string npcId)
  {
    string sql = "SELECT * FROM npcs WHERE id = @npcId;";
    return _db.QueryFirstOrDefault<Npc>(sql, new { npcId });
  }

  internal List<Npc> GetNpcsByCampaignId(string campaignId)
  {
    string sql = "SELECT * FROM npcs WHERE campaignId = @campaignId;";
    return _db.Query<Npc>(sql, new { campaignId }).ToList();
  }

  internal void CreateNpcByCampaignId(Npc npcData)
  {
    string sql = @"
      INSERT INTO npcs(id, name, picture, class, race, characterId, campaignId)
      VALUES(@Id, @Name, @Picture, @Class, @Race, @CharacterId, @CampaignId);";
    _db.Execute(sql, npcData);
  }

  internal void RemoveNpcByCampaignId(string npcId)
  {
    string sql = "DELETE FROM npcs WHERE id = @npcId LIMIT 1;";
    _db.Execute(sql, new { npcId });
  }
}