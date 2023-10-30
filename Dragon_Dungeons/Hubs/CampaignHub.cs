namespace Dragon_Dungeons.Hubs;

public class CampaignHub : Hub
{
  public async Task CampaignJoined(Player playerData)
  {
    await Clients.All.SendAsync("Player Joined Campaign", playerData);
  }
}