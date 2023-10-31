namespace Dragon_Dungeons.Hubs;

public interface ICampaignHub
{
  Task EnterCampaign(string campaignId);
  Task LeaveCampaign(string campaignId);
  Task PlayerJoinedCampaign(Player playerData);
}
