namespace Dragon_Dungeons.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignsController : ControllerBase
{
  private readonly CampaignsService _campaignsService;
  private readonly Auth0Provider _auth0Provider;

  public CampaignsController(CampaignsService campaignsService, Auth0Provider auth0Provider)
  {
    _campaignsService = campaignsService;
    _auth0Provider = auth0Provider;
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
  public async Task<ActionResult<Npc>> CreateNpc([FromBody] Npc npcData, string campaignId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      npcData.Id = Guid.NewGuid().ToString();
      npcData.CampaignId = campaignId;
      Npc npc = _campaignsService.CreateNpc(npcData, userInfo.Id);
      return Ok(npc);
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
}