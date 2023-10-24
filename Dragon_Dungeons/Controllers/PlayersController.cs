namespace Dragon_Dungeons.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
  private readonly PlayersService _playersService;
  private readonly Auth0Provider _auth0provider;

  public PlayersController(PlayersService playersService, Auth0Provider auth0provider)
  {
    _playersService = playersService;
    _auth0provider = auth0provider;
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Player>> CreatePlayer([FromBody] Player playerData)
  {
    try
    {
      Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
      playerData.CreatorId = userInfo.Id;
      playerData.Id = Guid.NewGuid().ToString();
      Player player = _playersService.CreatePlayer(playerData);
      return Ok(player);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}
