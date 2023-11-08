namespace Dragon_Dungeons.Hubs;

public interface ICampaignHub
{
  Task AwardXp(int xp);
  Task PlayerJoinedCampaign(Player playerData);
  Task PlayerLeftCampaign(Player playerData);
  Task CampaignNotes(string publicNotes, string campaignId);
  Task AddComment(Comment commentData);
  Task UpdateComment(Comment commentData);
  Task RemoveComment(string commentId);
  Task InitiateBattle(string initiative, string campaignId);
}
