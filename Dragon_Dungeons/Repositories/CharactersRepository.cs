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
        a.*
      FROM
        characters c
        JOIN accounts a ON a.id = c.creatorId;";
    return _db.Query<Character, Profile, Character>(
      sql,
      (character, profile) =>
      {
        character.Creator = profile;
        return character;
      }).ToList();
  }

  internal Character GetCharacterById(string characterId)
  {
    string sql = @"
      SELECT
        c.*,
        a.*
      FROM
        characters c
        JOIN accounts a ON a.id = c.creatorId
      WHERE c.id = @characterId;";
    return _db.Query<Character, Profile, Character>(
      sql,
      (character, profile) =>
      {
        character.Creator = profile;
        return character;
      },
      new { characterId }).FirstOrDefault();
  }

  internal List<Character> GetCharactersByUserId(string userId)
  {
    string sql = @"
      SELECT
        c.*,
        a.*
      FROM
        characters c
        JOIN accounts a ON a.id = c.creatorId
      WHERE c.creatorId = @userId; ";
    return _db.Query<Character, Profile, Character>(
      sql,
      (character, profile) =>
      {
        character.Creator = profile;
        return character;
      },
      new { userId }).ToList();
  }

  internal void CreateCharacter(Character characterData)
  {
    string sql = @"
      INSERT INTO characters(id, name, picture, hp, maxHp, speed, hitDie, class, race, alignment, age, feet, inches, weight, eyes, skin, hair, features, background, backstory, personalityTraits, ideals, bonds, flaws, manual, str, dex, con, intelligence, wis, cha, bonus, skills, proficiencies, cantrips, spells, casting, equipment, creatorId)
      VALUES(@Id, @Name, @Picture, @Hp, @MaxHp, @Speed, @HitDie, @Class, @Race, @Alignment, @Age, @Feet, @Inches, @Weight, @Eyes, @Skin, @Hair, @Features, @Background, @Backstory, @PersonalityTraits, @Ideals, @Bonds, @Flaws, @Manual, @Str, @Dex, @Con, @Intelligence, @Wis, @Cha, @Bonus, @Skills, @Proficiencies, @Cantrips, @Spells, @Casting, @Equipment, @CreatorId);";
    _db.Execute(sql, characterData);
  }

  internal void UpdateCharacter(Character characterData)
  {
    string sql = @"
      UPDATE characters SET
        hp = @Hp,
        tempHp = @TempHp,
        level = @Level,
        alignment = @Alignment,
        features = @Features,
        personalityTraits = @PersonalityTraits,
        ideals = @Ideals,
        bonds = @Bonds,
        flaws = @Flaws,
        bonus = @Bonus,
        skills = @Skills,
        proficiencies = @Proficiencies,
        cantrips = @Cantrips,
        spells = @Spells,
        casting = @Casting,
        equipment = @Equipment,
        armor = @Armor,
        weapons = @Weapons
      WHERE id = @Id LIMIT 1;";
    _db.Execute(sql, characterData);
  }

  internal void RemoveCharacter(string characterId)
  {
    string sql = "DELETE FROM characters WHERE id = @characterId LIMIT 1;";
    _db.Execute(sql, new { characterId });
  }
}