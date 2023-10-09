namespace Dragon_Dungeons.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharactersController : ControllerBase
{
  private readonly CharactersService _charactersService;
  private readonly Auth0Provider _auth0Provider;

  public CharactersController(CharactersService charactersService, Auth0Provider auth0Provider)
  {
    _charactersService = charactersService;
    _auth0Provider = auth0Provider;
  }

  [HttpGet]
  public ActionResult<List<Character>> GetCharacters()
  {
    try
    {
      List<Character> character = _charactersService.GetCharacters();
      return Ok(character);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("{characterId}")]
  public ActionResult<Character> GetCharacterById(string characterId)
  {
    try
    {
      Character character = _charactersService.GetCharacterById(characterId);
      return Ok(character);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Character>> CreateCharacter([FromBody] Character characterData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      characterData.CreatorId = userInfo.Id;
      characterData.Id = Guid.NewGuid().ToString();
      // characterData.Bonus.CharacterId = characterData.Id;
      // characterData.Bonus.Id = Guid.NewGuid().ToString();
      Character character = _charactersService.CreateCharacter(characterData);
      return Ok(character);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}
