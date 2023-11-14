namespace Dragon_Dungeons.Hubs;

public interface ICampaignHub
{
  Task PlayerJoinedCampaign(Player playerData);
  Task PlayerLeftCampaign(Player playerData);
  Task CampaignNotes(string publicNotes);
  Task AddComment(Comment commentData);
  Task UpdateComment(Comment commentData);
  Task RemoveComment(string commentId);
  Task InitiateBattle(string initiative);
  Task AwardXp(int xp);
  Task AwardPlayers(string award);
  Task Trade(object contents);
}
