namespace Dragon_Dungeons.Services;

public class CampaignsService(CampaignsRepository campaignsRepository, NpcsService npcsService, PlayersService playersService, CommentsService commentsService)
{
  private readonly CampaignsRepository _campaignsRepository = campaignsRepository;
  private readonly NpcsService _npcsService = npcsService;
  private readonly PlayersService _playersService = playersService;
  private readonly CommentsService _commentsService = commentsService;

  internal Campaign GetCampaignById(string campaignId, string userId)
  {
    Campaign campaign = _campaignsRepository.GetCampaignById(campaignId) ?? throw new Exception($"[NO CAMPAIGN MATCHES THE ID {campaignId}]");
    campaign.Players = _playersService.GetPlayersByCampaignId(campaignId);
    campaign.Comments = _commentsService.GetCommentsByCampaignId(campaignId);
    if (campaign.CreatorId != userId)
    {
      campaign.PrivateNotes = null;
      campaign.Events = null;
      campaign.Monsters = null;
      campaign.Players = campaign.Players.Where(p => p.CreatorId == userId).ToList();
    }
    else
    {
      campaign.Npcs = _npcsService.GetNpcsByCampaignId(campaignId);
    }
    return campaign;
  }

  internal List<Campaign> GetCampaignsByUserId(string userId)
  {
    List<Campaign> campaigns = _campaignsRepository.GetCampaignsByUserId(userId);
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
      _npcsService.CreateNpcByCampaignId(n);
    });
    Campaign campaign = GetCampaignById(campaignData.Id, campaignData.CreatorId);
    return campaign;
  }

  internal Npc CreateNpcByCampaignId(Npc npcData, string userId)
  {
    Campaign campaign = GetCampaignById(npcData.CampaignId, userId);
    if (campaign.CreatorId != userId)
    {
      throw new Exception($"[YOU ARE NOT THE CREATOR OF {campaign.Name}]");
    }
    return _npcsService.CreateNpcByCampaignId(npcData);
  }

  internal Campaign CreatePlayerByCampaignId(Player playerData)
  {
    Campaign campaign = GetCampaignById(playerData.CampaignId, playerData.CreatorId);
    if (campaign.CreatorId == playerData.CreatorId)
    {
      throw new Exception("[YOU CANNOT JOIN A CAMPAIGN YOU OWN]");
    }
    if (campaign.Players.Count > 0)
    {
      throw new Exception("[YOU ARE ALREADY IN THIS CAMPAIGN]");
    }
    _playersService.CreatePlayerByCampaignId(playerData);
    return campaign;
  }

  internal Campaign UpdateCampaign(Campaign campaignData)
  {
    Campaign originalCampaign = GetCampaignById(campaignData.Id, campaignData.CreatorId);
    if (campaignData.CreatorId == originalCampaign.CreatorId)
    {
      originalCampaign.Description = campaignData.Description ?? originalCampaign.Description;
      originalCampaign.PrivateNotes = campaignData.PrivateNotes ?? originalCampaign.PrivateNotes;
      originalCampaign.PublicNotes = campaignData.PublicNotes ?? originalCampaign.PublicNotes;
      originalCampaign.Events = campaignData.Events ?? originalCampaign.Events;
      originalCampaign.Monsters = campaignData.Monsters ?? originalCampaign.Monsters;
      originalCampaign.Initiative = campaignData.Initiative ?? originalCampaign.Initiative;
      _campaignsRepository.UpdateCampaign(originalCampaign);
    }
    else
    {
      originalCampaign.Initiative = campaignData.Initiative ?? originalCampaign.Initiative;
      _campaignsRepository.NoAuthUpdateCampaign(originalCampaign);
    }
    return originalCampaign;
  }

  internal Campaign RemoveCampaign(string campaignId, string userId)
  {
    Campaign campaignToDelete = HandleData(campaignId, userId);
    _campaignsRepository.RemoveCampaign(campaignId);
    return campaignToDelete;
  }

  internal Npc RemoveNpcByCampaignId(string campaignId, string npcId, string userId)
  {
    Campaign campaign = HandleData(campaignId, userId);
    return _npcsService.RemoveNpcByCampaignId(npcId, userId, campaign.CreatorId);
  }

  internal Player RemovePlayerByCampaignId(string campaignId, string playerId, string userId)
  {
    Campaign campaign = GetCampaignById(campaignId, userId);
    return _playersService.RemovePlayerByCampaignId(playerId, userId, campaign.CreatorId);
  }

  internal Comment RemoveCommentByCampaignId(string campaignId, string commentId, string userId)
  {
    Campaign campaign = GetCampaignById(campaignId, userId);
    return _commentsService.RemoveCommentByCampaignId(commentId, userId, campaign.CreatorId);
  }

  private Campaign HandleData(string campaignId, string userId)
  {
    Campaign campaignData = GetCampaignById(campaignId, userId);
    if (campaignData.CreatorId != userId)
    {
      throw new Exception($"[YOU ARE NOT THE CREATOR OF {campaignData.Name}]");
    }
    return campaignData;
  }
}