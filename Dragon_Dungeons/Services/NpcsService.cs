namespace Dragon_Dungeons.Services;

public class NpcsService
{
  private readonly NpcsRepository _npcsRepository;

  public NpcsService(NpcsRepository npcsRepository)
  {
    _npcsRepository = npcsRepository;
  }

  internal List<Npc> GetNpcsByCampaignId(string campaignId)
  {
    return _npcsRepository.GetNpcsByCampaignId(campaignId);
  }

  internal void CreateNpc(Npc npcData)
  {
    _npcsRepository.CreateNpc(npcData);
  }
}
