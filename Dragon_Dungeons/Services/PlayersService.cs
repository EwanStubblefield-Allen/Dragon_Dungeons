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

  internal List<Campaign> GetPlayersByUserId(string userId)
  {
    return _playersRepository.GetPlayersByUserId(userId);
  }

  internal void CreatePlayerByCampaignId(Player playerData)
  {
    _playersRepository.CreatePlayerByCampaignId(playerData);
  }

  internal Player RemovePlayerByCampaignId(string playerId, string userId, string campaignCreatorId)
  {
    Player playerToDelete = GetPlayerById(playerId);
    if (playerToDelete.CreatorId != userId && campaignCreatorId != userId)
    {
      throw new Exception($"[YOU ARE NOT THE CREATOR OF {playerToDelete.Name}]");
    }
    _playersRepository.RemovePlayerByCampaignId(playerId);
    return playerToDelete;
  }
}
