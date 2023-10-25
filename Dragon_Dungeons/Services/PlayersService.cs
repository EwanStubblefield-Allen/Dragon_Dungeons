namespace Dragon_Dungeons.Services;

public class PlayersService
{
  private readonly PlayersRepository _playersRepository;

  public PlayersService(PlayersRepository playersRepository)
  {
    _playersRepository = playersRepository;
  }

  internal Player GetPlayerById(string playerId)
  {
    Player player = _playersRepository.GetPlayerById(playerId);
    return player ?? throw new Exception($"[NO PLAYER MATCHES THE ID {playerId}]");
  }

  internal List<Player> GetPlayersByCampaignId(string campaignId)
  {
    return _playersRepository.GetPlayersByCampaignId(campaignId);
  }

  internal List<Player> GetPlayerByCampaignIdAndUserId(string campaignId, string userId)
  {
    return _playersRepository.GetPlayerByCampaignIdAndUserId(campaignId, userId);
  }

  internal Player CreatePlayerByCampaignId(Player playerData)
  {
    List<Player> players = GetPlayersByCampaignId(playerData.CampaignId);
    bool foundPlayer = players.Exists(p => p.CreatorId == playerData.CreatorId);
    if (foundPlayer)
    {
      throw new Exception("[YOU ARE ALREADY IN THIS CAMPAIGN]");
    }
    _playersRepository.CreatePlayerByCampaignId(playerData);
    return GetPlayerById(playerData.Id);
  }

  internal Player RemovePlayerByCampaignId(string playerId, string userId, string campaignCreatorId)
  {
    Player playerToDelete = GetPlayerById(playerId);
    if (playerToDelete.CreatorId != userId || campaignCreatorId != userId)
    {
      throw new Exception($"[YOU ARE NOT THE CREATOR OF {playerToDelete.Name}]");
    }
    _playersRepository.RemovePlayerByCampaignId(playerId);
    return playerToDelete;
  }
}
