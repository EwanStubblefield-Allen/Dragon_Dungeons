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

  internal Npc CreateNpc(Npc npcData)
  {
    _npcsRepository.CreateNpc(npcData);
    return GetNpcById(npcData.Id);
  }
}