namespace Dragon_Dungeons.Repositories;

public class NpcsRepository
{
  private readonly IDbConnection _db;

  public NpcsRepository(IDbConnection db)
  {
    _db = db;
  }

  internal List<Npc> GetNpcsByCampaignId(string campaignId)
  {
    string sql = "SELECT * FROM npcs WHERE campaignId = @campaignId;";
    return _db.Query<Npc>(sql, new { campaignId }).ToList();
  }

  internal void CreateNpc(Npc npcData)
  {
    string sql = @"
      INSERT INTO npcs(name, picture, characterId, campaignId)
      VALUES(@Name, @Picture, @CharacterId, @CampaignId);";
    _db.Execute(sql, npcData);
  }
}