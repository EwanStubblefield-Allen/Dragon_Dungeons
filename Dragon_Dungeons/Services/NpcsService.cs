namespace Dragon_Dungeons.Services;

public class NpcsService
{
  private readonly NpcsRepository _npcsRepository;

  public NpcsService(NpcsRepository npcsRepository)
  {
    _npcsRepository = npcsRepository;
  }

  internal Npc GetNpcById(string npcId)
  {
    Npc npc = _npcsRepository.GetNpcById(npcId);
    return npc ?? throw new Exception($"[NO NPC MATCHES THE ID {npcId}]");
  }

  internal List<Npc> GetNpcsByCampaignId(string campaignId)
  {
    return _npcsRepository.GetNpcsByCampaignId(campaignId);
  }

  internal Npc CreateNpcByCampaignId(Npc npcData)
  {
    _npcsRepository.CreateNpcByCampaignId(npcData);
    return GetNpcById(npcData.Id);
  }

  internal Npc RemoveNpcByCampaignId(string npcId, string userId, string campaignCreatorId)
  {
    Npc npcToRemove = GetNpcById(npcId);
    if (campaignCreatorId != userId)
    {
      throw new Exception($"[YOU ARE NOT THE CREATOR OF {npcToRemove.Name}]");
    }
    _npcsRepository.RemoveNpcByCampaignId(npcId);
    return npcToRemove;
  }
}