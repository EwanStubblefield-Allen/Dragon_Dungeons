namespace Dragon_Dungeons.Hubs;

public class CampaignHub : Hub<ICampaignHub>
{
  public async Task EnterGroup(string groupId)
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
  }

  public async Task LeaveGroup(string groupId)
  {
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
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
    await Clients.Group(campaignId).CampaignNotes(publicNotes);
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

  public async Task InitiateBattle(string campaignId, string initiative)
  {
    await Clients.Group(campaignId).InitiateBattle(initiative);
  }

  public async Task AwardXp(string campaignId, int xp)
  {
    await Clients.OthersInGroup(campaignId).AwardXp(xp);
  }

  public async Task AwardPlayers(string campaignId, string award)
  {
    await Clients.OthersInGroup(campaignId).AwardPlayers(award);
  }

  public async Task Trade(string location, object contents)
  {
    await Clients.Group(location).Trade(contents);
  }
}