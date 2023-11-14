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

  internal Character UpdateCharacter(Character characterData)
  {
    Character originalCharacter = HandleData(characterData.Id, characterData.CreatorId);
    originalCharacter.Hp = characterData.Hp ?? originalCharacter.Hp;
    originalCharacter.TempHp = characterData.TempHp ?? originalCharacter.TempHp;
    originalCharacter.ArmorClass = characterData.ArmorClass ?? originalCharacter.ArmorClass;
    originalCharacter.Level = characterData.Level ?? originalCharacter.Level;
    originalCharacter.Xp = characterData.Xp ?? originalCharacter.Xp;
    originalCharacter.Alignment = characterData.Alignment ?? originalCharacter.Alignment;
    originalCharacter.Features = characterData.Features ?? originalCharacter.Features;
    originalCharacter.PersonalityTraits = characterData.PersonalityTraits ?? originalCharacter.PersonalityTraits;
    originalCharacter.Ideals = characterData.Ideals ?? originalCharacter.Ideals;
    originalCharacter.Bonds = characterData.Bonds ?? originalCharacter.Bonds;
    originalCharacter.Flaws = characterData.Flaws ?? originalCharacter.Flaws;
    originalCharacter.Manual = characterData.Manual ?? originalCharacter.Manual;
    originalCharacter.Str = characterData.Str ?? originalCharacter.Str;
    originalCharacter.Dex = characterData.Dex ?? originalCharacter.Dex;
    originalCharacter.Con = characterData.Con ?? originalCharacter.Con;
    originalCharacter.Intelligence = characterData.Intelligence ?? originalCharacter.Intelligence;
    originalCharacter.Wis = characterData.Wis ?? originalCharacter.Wis;
    originalCharacter.Cha = characterData.Cha ?? originalCharacter.Cha;
    originalCharacter.Bonus = characterData.Bonus ?? originalCharacter.Bonus;
    originalCharacter.Skills = characterData.Skills ?? originalCharacter.Skills;
    originalCharacter.Proficiencies = characterData.Proficiencies ?? originalCharacter.Proficiencies;
    originalCharacter.CharFeatures = characterData.CharFeatures ?? originalCharacter.CharFeatures;
    originalCharacter.Cantrips = characterData.Cantrips ?? originalCharacter.Cantrips;
    originalCharacter.Spells = characterData.Spells ?? originalCharacter.Spells;
    originalCharacter.Casting = characterData.Casting ?? originalCharacter.Casting;
    originalCharacter.Equipment = characterData.Equipment ?? originalCharacter.Equipment;
    originalCharacter.Currency = characterData.Currency ?? originalCharacter.Currency;
    originalCharacter.Armor = characterData.Armor ?? originalCharacter.Armor;
    originalCharacter.Weapons = characterData.Weapons ?? originalCharacter.Weapons;
    _charactersRepository.UpdateCharacter(originalCharacter);
    return originalCharacter;
  }

  internal Character RemoveCharacter(string characterId, string userId)
  {
    Character characterToRemove = HandleData(characterId, userId);
    _charactersRepository.RemoveCharacter(characterId);
    return characterToRemove;
  }

  private Character HandleData(string characterId, string userId)
  {
    Character characterData = GetCharacterById(characterId);
    if (characterData.CreatorId != userId)
    {
      throw new Exception($"[YOU ARE NOT THE CREATOR OF {characterData.Name}]");
    }
    return characterData;
  }
}