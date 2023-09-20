namespace Dragon_Dungeons.Services;

public class CharactersService
{
  private readonly CharactersRepository _charactersRepository;

  public CharactersService(CharactersRepository charactersRepository)
  {
    _charactersRepository = charactersRepository;
  }

  internal List<Character> GetCharacters()
  {
    return _charactersRepository.GetCharacters();
  }

  internal Character GetCharacterById(string characterId)
  {
    return _charactersRepository.GetCharacterById(characterId);
  }

  internal Character CreateCharacter(Character characterData)
  {
    _charactersRepository.CreateCharacter(characterData);
    return GetCharacterById(characterData.Id);
  }
}
