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

  internal Npc RemoveNpcByCampaignId(string npcId)
  {
    Npc npcToRemove = GetNpcById(npcId);
    _npcsRepository.RemoveNpcByCampaignId(npcId);
    return npcToRemove;
  }
}