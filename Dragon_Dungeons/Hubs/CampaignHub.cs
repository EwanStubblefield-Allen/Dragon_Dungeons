namespace Dragon_Dungeons.Hubs;

public class CampaignHub : Hub<ICampaignHub>
{
  public async Task EnterCampaign(string campaignId)
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, campaignId);
  }

  public async Task LeaveCampaign(string campaignId)
  {
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, campaignId);
  }

  public async Task PlayerJoinedCampaign(Player playerData)
  {
    await Clients.All.PlayerJoinedCampaign(playerData);
  }
}