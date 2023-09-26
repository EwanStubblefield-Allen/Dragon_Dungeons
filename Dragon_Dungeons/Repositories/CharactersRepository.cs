namespace Dragon_Dungeons.Repositories;

public class CharactersRepository
{
  private readonly IDbConnection _db;

  public CharactersRepository(IDbConnection db)
  {
    _db = db;
  }

  internal List<Character> GetCharacters()
  {
    string sql = @"
      SELECT
        c.*,
        a.*,
        b.*
      FROM
        characters c
        JOIN accounts a ON a.id = c.creatorId
        LEFT JOIN bonuses b ON c.id = b.characterId;";
    return _db.Query<Character, Profile, Bonus, Character>(
      sql,
      (character, profile, bonus) =>
      {
        character.Creator = profile;
        character.Bonus = bonus;
        return character;
      }).ToList();
  }

  internal Character GetCharacterById(string characterId)
  {
    string sql = @"
      SELECT
        c.*,
        a.*,
        b.*
      FROM
        characters c
        JOIN accounts a ON a.id = c.creatorId
        LEFT JOIN bonuses b ON c.id = b.characterId
      WHERE c.id = @characterId;";
    return _db.Query<Character, Profile, Bonus, Character>(
      sql,
      (character, profile, bonus) =>
      {
        character.Creator = profile;
        character.Bonus = bonus;
        return character;
      },
      new { characterId }).FirstOrDefault();
  }

  internal List<Character> GetCharactersByUserId(string userId)
  {
    string sql = @"
      SELECT
        c.*,
        a.*,
        b.*
      FROM
        characters c
        JOIN accounts a ON a.id = c.creatorId
        LEFT JOIN bonuses b ON c.id = b.characterId
      WHERE c.creatorId = @userId; ";
    return _db.Query<Character, Profile, Bonus, Character>(
      sql,
      (character, profile, bonus) =>
      {
        character.Creator = profile;
        character.Bonus = bonus;
        return character;
      },
      new { userId }).ToList();
  }

  internal void CreateCharacter(Character characterData)
  {
    string sql = @"
      INSERT INTO characters(id, name, class, race, alignment, age, feet, inches, weight, eyes, skin, hair, features, background, backstory, personalityTraits, ideals, bonds, flaws, manual, str, dex, con, intelligence, wis, cha, skills, proficiencies, cantrips, spells, equipment, creatorId)
      VALUES(@Id, @Name, @Class, @Race, @Alignment, @Age, @Feet, @Inches, @Weight, @Eyes, @Skin, @Hair, @Features, @Background, @Backstory, @PersonalityTraits, @Ideals, @Bonds, @Flaws, @Manual, @Str, @Dex, @Con, @Intelligence, @Wis, @Cha, @Skills, @Proficiencies, @Cantrips, @Spells, @Equipment, @CreatorId);";
    _db.Execute(sql, characterData);
  }
}
