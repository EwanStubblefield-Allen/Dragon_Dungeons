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
    Character character = _charactersRepository.GetCharacterById(characterId);
    return character ?? throw new Exception($"[NO CHARACTER MATCHES THE ID {characterId}]");
  }

  internal List<Character> GetCharactersByUserId(string userId)
  {
    return _charactersRepository.GetCharactersByUserId(userId);
  }

  internal Character CreateCharacter(Character characterData)
  {
    _charactersRepository.CreateCharacter(characterData);
    return GetCharacterById(characterData.Id);
  }
}
