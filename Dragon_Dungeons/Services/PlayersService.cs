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

  internal Player CreatePlayer(Player playerData)
  {
    string playerId = _playersRepository.CreatePlayer(playerData);
    return GetPlayerById(playerId);
  }
}
