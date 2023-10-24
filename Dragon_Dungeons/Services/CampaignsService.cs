namespace Dragon_Dungeons.Services;

public class CampaignsService
{
  private readonly CampaignsRepository _campaignsRepository;
  private readonly NpcsService _npcsService;
  private readonly PlayersService _playersService;

  public CampaignsService(CampaignsRepository campaignsRepository, NpcsService npcsService, PlayersService playersService)
  {
    _campaignsRepository = campaignsRepository;
    _npcsService = npcsService;
    _playersService = playersService;
  }

  internal Campaign GetCampaignById(string campaignId, string userId)
  {
    Campaign campaign = _campaignsRepository.GetCampaignById(campaignId) ?? throw new Exception($"[NO CAMPAIGN MATCHES THE ID {campaignId}]");
    if (campaign.CreatorId != userId)
    {
      campaign.PrivateNote = null;
      campaign.Events = null;
      campaign.Monsters = null;
    }
    else
    {
      campaign.Npcs ??= _npcsService.GetNpcsByCampaignId(campaign.Id);
      campaign.Players = _playersService.GetPlayersByCampaignId(campaign.Id);
    }
    return campaign;
  }

  internal List<Campaign> GetCampaignsByUserId(string userId)
  {
    List<Campaign> campaigns = _campaignsRepository.GetCampaignsByUserId(userId);
    campaigns.ForEach(c => c.Npcs = _npcsService.GetNpcsByCampaignId(c.Id));
    return campaigns;
  }

  internal Campaign CreateCampaign([FromBody] Campaign campaignData)
  {
    _campaignsRepository.CreateCampaign(campaignData);
    campaignData.Npcs?.ForEach(n =>
    {
      n.CharacterId = n.Id;
      n.CampaignId = campaignData.Id;
      n.Id = Guid.NewGuid().ToString();
      campaignData.Npcs.Add(_npcsService.CreateNpc(n));
    });
    Campaign campaign = GetCampaignById(campaignData.Id, campaignData.CreatorId);
    return campaign;
  }

  internal Npc CreateNpc(Npc npcData, string userId)
  {
    Campaign campaign = GetCampaignById(npcData.CampaignId, userId);
    if (campaign.CreatorId != userId)
    {
      throw new Exception($"[YOU ARE NOT THE CREATOR OF {campaign.Name}]");
    }
    return _npcsService.CreateNpc(npcData);
  }
}