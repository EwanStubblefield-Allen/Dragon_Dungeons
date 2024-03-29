namespace Dragon_Dungeons.Repositories;

public class CharactersRepository(IDbConnection db)
{
  private readonly IDbConnection _db = db;

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
      INSERT INTO characters(id, name, picture, hp, maxHp, armorClass, speed, hitDie, class, race, alignment, age, feet, inches, weight, eyes, skin, hair, features, background, backstory, personalityTraits, ideals, bonds, flaws, manual, str, dex, con, intelligence, wis, cha, bonus, skills, proficiencies, charFeatures, cantrips, spells, casting, equipment, currency, creatorId)
      VALUES(@Id, @Name, @Picture, @Hp, @MaxHp, @ArmorClass, @Speed, @HitDie, @Class, @Race, @Alignment, @Age, @Feet, @Inches, @Weight, @Eyes, @Skin, @Hair, @Features, @Background, @Backstory, @PersonalityTraits, @Ideals, @Bonds, @Flaws, @Manual, @Str, @Dex, @Con, @Intelligence, @Wis, @Cha, @Bonus, @Skills, @Proficiencies, @CharFeatures, @Cantrips, @Spells, @Casting, @Equipment, @Currency, @CreatorId);";
    _db.Execute(sql, characterData);
  }

  internal void UpdateCharacter(Character characterData)
  {
    string sql = @"
      UPDATE characters SET
        hp = @Hp,
        tempHp = @TempHp,
        armorClass = @ArmorClass,
        level = @Level,
        xp = @Xp,
        alignment = @Alignment,
        features = @Features,
        personalityTraits = @PersonalityTraits,
        ideals = @Ideals,
        bonds = @Bonds,
        flaws = @Flaws,
        manual = @Manual,
        str = @Str,
        dex = @Dex,
        con = @Con,
        intelligence = @Intelligence,
        wis = @Wis,
        cha = @Cha,
        bonus = @Bonus,
        skills = @Skills,
        proficiencies = @Proficiencies,
        charFeatures = @CharFeatures,
        cantrips = @Cantrips,
        spells = @Spells,
        casting = @Casting,
        equipment = @Equipment,
        currency = @Currency,
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