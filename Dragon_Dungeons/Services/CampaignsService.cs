namespace Dragon_Dungeons.Services;

public class CampaignsService
{
  private readonly CampaignsRepository _campaignsRepository;
  private readonly NpcsService _npcsService;

  public CampaignsService(CampaignsRepository campaignsRepository, NpcsService npcsService)
  {
    _campaignsRepository = campaignsRepository;
    _npcsService = npcsService;
  }

  internal Campaign GetCampaignById(string campaignId)
  {
    Campaign campaign = _campaignsRepository.GetCampaignById(campaignId) ?? throw new Exception($"[NO CAMPAIGN MATCHES THE ID {campaignId}]");
    campaign.Npcs = _npcsService.GetNpcsByCampaignId(campaign.Id);
    return campaign;
  }

  internal Campaign CreateCampaign(Campaign campaignData)
  {
    _campaignsRepository.CreateCampaign(campaignData);
    campaignData.Npcs.ForEach(n => _npcsService.CreateNpc(n));
    Campaign campaign = GetCampaignById(campaignData.Id);
    return campaign;
  }
}
