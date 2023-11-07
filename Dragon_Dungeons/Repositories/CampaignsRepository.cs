namespace Dragon_Dungeons.Repositories;

public class CampaignsRepository
{
  private readonly IDbConnection _db;

  public CampaignsRepository(IDbConnection db)
  {
    _db = db;
  }

  internal Campaign GetCampaignById(string campaignId)
  {
    string sql = @"
      SELECT
        c.*,
        a.*
      FROM
        campaigns c
        JOIN accounts a ON a.id = c.creatorId
      WHERE c.id = @campaignId;";
    return _db.Query<Campaign, Account, Campaign>(
      sql,
      (campaign, profile) =>
      {
        campaign.Creator = profile;
        return campaign;
      },
      new { campaignId }).FirstOrDefault();
  }

  internal List<Campaign> GetCampaignsByUserId(string userId)
  {
    string sql = @"
      SELECT
        c.*,
        a.*
      FROM
        campaigns c
        JOIN accounts a ON a.id = c.creatorId
      WHERE c.creatorId = @userId;";
    return _db.Query<Campaign, Account, Campaign>(
      sql,
      (campaign, profile) =>
      {
        campaign.Creator = profile;
        return campaign;
      },
      new { userId }).ToList();
  }

  internal void CreateCampaign(Campaign campaignData)
  {
    string sql = @"
      INSERT INTO campaigns(id, name, description, privateNotes, publicNotes, events, monsters, creatorId)
      VALUES(@Id, @Name, @Description, @PrivateNotes, @PublicNotes, @Events, @Monsters, @CreatorId);";
    _db.Execute(sql, campaignData);
  }

  internal void UpdateCampaign(Campaign campaignData)
  {
    string sql = @"
      UPDATE campaigns SET
        description = @Description,
        PrivateNotes = @PrivateNotes,
        PublicNotes = @PublicNotes,
        events = @Events,
        monsters = @Monsters,
        initiative = @Initiative
      WHERE id = @Id LIMIT 1;";
    _db.Execute(sql, campaignData);
  }

  internal void NoAuthUpdateCampaign(Campaign campaignData)
  {
    string sql = @"
      UPDATE campaigns SET
        initiative = @Initiative
      WHERE id = @Id LIMIT 1;";
    _db.Execute(sql, campaignData);
  }

  internal void RemoveCampaign(string campaignId)
  {
    string sql = "DELETE FROM campaigns WHERE id = @campaignId LIMIT 1;";
    _db.Execute(sql, new { campaignId });
  }
}