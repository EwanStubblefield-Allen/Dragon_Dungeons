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
      INSERT INTO campaigns(id, name, description, privateNote, publicNote, events, monsters, creatorId)
      VALUES(@Id, @Name, @Description, @PrivateNote, @PublicNote, @Events, @Monsters, @CreatorId);";
    _db.Execute(sql, campaignData);
  }
}