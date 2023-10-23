namespace Dragon_Dungeons.Services;

public class PlayersService
{
  private readonly PlayersRepository _playersRepository;

  public PlayersService(PlayersRepository playersRepository)
  {
    _playersRepository = playersRepository;
  }

  internal List<Player> GetPlayersByCampaignId(string campaignId)
  {
    return _playersRepository.GetPlayersByCampaignId(campaignId);
  }
}
