using System.Numerics;

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

  public async Task AwardXp(string campaignId, int xp)
  {
    await Clients.OthersInGroup(campaignId).AwardXp(xp);
  }

  public async Task InitiateBattle(string initiative, string campaignId)
  {
    await Clients.Group(campaignId).InitiateBattle(initiative, campaignId);
  }

  public async Task PlayerJoinedCampaign(Player playerData)
  {
    await Clients.Group(playerData.CampaignId).PlayerJoinedCampaign(playerData);
  }

  public async Task PlayerLeftCampaign(Player playerData)
  {
    await Clients.Group(playerData.CampaignId).PlayerLeftCampaign(playerData);
  }

  public async Task CampaignNotes(string publicNotes, string campaignId)
  {
    await Clients.Group(campaignId).CampaignNotes(publicNotes, campaignId);
  }

  public async Task AddComment(Comment commentData)
  {
    await Clients.Group(commentData.CampaignId).AddComment(commentData);
  }

  public async Task UpdateComment(Comment commentData)
  {
    await Clients.Group(commentData.CampaignId).UpdateComment(commentData);
  }

  public async Task RemoveComment(Comment commentData)
  {
    await Clients.Group(commentData.CampaignId).RemoveComment(commentData.Id);
  }
}