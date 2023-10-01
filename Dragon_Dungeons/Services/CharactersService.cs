namespace Dragon_Dungeons.Services;

public class CharactersService
{
  private readonly CharactersRepository _charactersRepository;
  private readonly BonusesService _bonusesService;

  public CharactersService(CharactersRepository charactersRepository, BonusesService bonusesService)
  {
    _charactersRepository = charactersRepository;
    _bonusesService = bonusesService;
  }

  internal List<Character> GetCharacters()
  {
    return _charactersRepository.GetCharacters();
  }

  internal Character GetCharacterById(string characterId)
  {
    return _charactersRepository.GetCharacterById(characterId);
  }

  internal List<Character> GetCharactersByUserId(string userId)
  {
    return _charactersRepository.GetCharactersByUserId(userId);
  }

  internal Character CreateCharacter(Character characterData)
  {
    _charactersRepository.CreateCharacter(characterData);
    _bonusesService.CreateBonus(characterData.Bonus);
    return GetCharacterById(characterData.Id);
  }
}
