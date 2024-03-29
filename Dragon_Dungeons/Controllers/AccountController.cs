namespace Dragon_Dungeons.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(AccountService accountService, CharactersService charactersService, CampaignsService campaignsService, PlayersService playersService, Auth0Provider auth0Provider) : ControllerBase
{
  private readonly AccountService _accountService = accountService;
  private readonly CharactersService _charactersService = charactersService;
  private readonly CampaignsService _campaignsService = campaignsService;
  private readonly PlayersService _playersService = playersService;
  private readonly Auth0Provider _auth0Provider = auth0Provider;

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

  [HttpGet("characters")]
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

  [HttpGet("campaigns")]
  [Authorize]
  public async Task<ActionResult<List<Campaign>>> GetCampaignsByUserId()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      List<Campaign> campaigns = _campaignsService.GetCampaignsByUserId(userInfo.Id);
      return Ok(campaigns);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("players")]
  [Authorize]
  public async Task<ActionResult<List<Campaign>>> GetPlayersByUserId()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      List<Campaign> campaigns = _playersService.GetPlayersByUserId(userInfo.Id);
      return Ok(campaigns);
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