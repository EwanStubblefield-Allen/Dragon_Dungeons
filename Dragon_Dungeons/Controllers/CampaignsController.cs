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
  public ActionResult<Campaign> GetCampaignById(string campaignId)
  {
    try
    {
      Campaign campaign = _campaignsService.GetCampaignById(campaignId);
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
      campaignData.Npcs?.ConvertAll(n =>
      {
        n.CharacterId = n.Id;
        n.CampaignId = campaignData.Id;
        return n;
      });
      Campaign campaign = _campaignsService.CreateCampaign(campaignData);
      return Ok(campaign);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}