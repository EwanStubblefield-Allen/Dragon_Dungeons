namespace Dragon_Dungeons.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
  private readonly AccountService _accountService;
  private readonly CharactersService _charactersService;
  private readonly Auth0Provider _auth0Provider;

  public AccountController(AccountService accountService, CharactersService charactersService, Auth0Provider auth0Provider)
  {
    _accountService = accountService;
    _charactersService = charactersService;
    _auth0Provider = auth0Provider;
  }

  [HttpGet]
  [Authorize]
  public async Task<ActionResult<Account>> Get()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      return Ok(_accountService.GetOrCreateProfile(userInfo));
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("{characters}")]
  [Authorize]
  public async Task<ActionResult<List<Character>>> GetCharactersByUserId()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      List<Character> Characters = _charactersService.GetCharactersByUserId(userInfo.Id);
      return Ok(Characters);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPut]
  [Authorize]
  public async Task<ActionResult<Account>> Edit([FromBody] Account userData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

      Account account = _accountService.Edit(userData, userInfo.Email);
      return Ok(account);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

}
