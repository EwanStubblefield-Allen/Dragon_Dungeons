namespace Dragon_Dungeons.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignsController : ControllerBase
{
  private readonly CampaignsService _campaignsService;
  private readonly Auth0Provider _auth0Provider;
  private readonly IHubContext<CampaignHub, ICampaignHub> _hubContext;

  public CampaignsController(CampaignsService campaignsService, Auth0Provider auth0Provider, IHubContext<CampaignHub, ICampaignHub> hubContext)
  {
    _campaignsService = campaignsService;
    _auth0Provider = auth0Provider;
    _hubContext = hubContext;
  }

  [HttpGet("{campaignId}")]
  public async Task<ActionResult<Campaign>> GetCampaignById(string campaignId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      Campaign campaign = _campaignsService.GetCampaignById(campaignId, userInfo.Id);
      return Ok(campaign);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Campaign>> CreateCampaign([FromBody] Campaign campaignData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      campaignData.CreatorId = userInfo.Id;
      campaignData.Id = Guid.NewGuid().ToString();
      Campaign campaign = _campaignsService.CreateCampaign(campaignData);
      return Ok(campaign);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPost("{campaignId}/npcs")]
  [Authorize]
  public async Task<ActionResult<Npc>> CreateNpcByCampaignId([FromBody] Npc npcData, string campaignId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      npcData.Id = Guid.NewGuid().ToString();
      npcData.CampaignId = campaignId;
      Npc npc = _campaignsService.CreateNpcByCampaignId(npcData, userInfo.Id);
      return Ok(npc);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPost("{campaignId}/players")]
  [Authorize]
  public async Task<ActionResult<Campaign>> CreatePlayerByCampaignId([FromBody] Player playerData, string campaignId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      playerData.CreatorId = userInfo.Id;
      playerData.Id = Guid.NewGuid().ToString();
      playerData.CampaignId = campaignId;
      Campaign campaign = _campaignsService.CreatePlayerByCampaignId(playerData);
      await _hubContext.Clients.All.PlayerJoinedCampaign(playerData);
      return Ok(campaign);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPut("{campaignId}")]
  [Authorize]
  public async Task<ActionResult<Campaign>> UpdateCampaign([FromBody] Campaign campaignData, string campaignId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      campaignData.CreatorId = userInfo.Id;
      campaignData.Id = campaignId;
      Campaign campaign = _campaignsService.UpdateCampaign(campaignData);
      return Ok(campaign);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpDelete("{campaignId}")]
  [Authorize]
  public async Task<ActionResult<Campaign>> RemoveCampaign(string campaignId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      Campaign campaign = _campaignsService.RemoveCampaign(campaignId, userInfo.Id);
      return Ok(campaign);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpDelete("{campaignId}/npcs/{npcId}")]
  [Authorize]
  public async Task<ActionResult<Npc>> RemoveNpcByCampaignId(string campaignId, string npcId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      Npc npc = _campaignsService.RemoveNpcByCampaignId(campaignId, npcId, userInfo.Id);
      return Ok(npc);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpDelete("{campaignId}/players/{playerId}")]
  [Authorize]
  public async Task<ActionResult<Player>> RemovePlayerByCampaignId(string campaignId, string playerId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      Player player = _campaignsService.RemovePlayerByCampaignId(campaignId, playerId, userInfo.Id);
      return Ok(player);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}