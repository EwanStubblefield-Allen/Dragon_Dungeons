namespace Dragon_Dungeons.Services;

public class BonusesService
{
  private readonly BonusesRepository _bonusesRepository;

  public BonusesService(BonusesRepository bonusesRepository)
  {
    _bonusesRepository = bonusesRepository;
  }

  internal void CreateBonus(Bonus bonusData)
  {
    _bonusesRepository.CreateBonus(bonusData);
  }
}