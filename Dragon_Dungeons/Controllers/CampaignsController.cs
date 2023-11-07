namespace Dragon_Dungeons.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignsController : ControllerBase
{
  private readonly CampaignsService _campaignsService;
  private readonly CommentsService _commentsService;
  private readonly Auth0Provider _auth0Provider;
  private readonly IHubContext<CampaignHub, ICampaignHub> _hubContext;

  public CampaignsController(CampaignsService campaignsService, CommentsService commentsService, Auth0Provider auth0Provider, IHubContext<CampaignHub, ICampaignHub> hubContext)
  {
    _campaignsService = campaignsService;
    _commentsService = commentsService;
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
      await _hubContext.Clients.Group(campaignId).PlayerJoinedCampaign(playerData);
      return Ok(campaign);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPost("{campaignId}/comments")]
  [Authorize]
  public async Task<ActionResult<Comment>> CreateCommentByCampaignId([FromBody] Comment commentData, string campaignId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      commentData.CreatorId = userInfo.Id;
      commentData.Id = Guid.NewGuid().ToString();
      commentData.CampaignId = campaignId;
      Comment comment = _commentsService.CreateCommentByCampaignId(commentData);
      await _hubContext.Clients.Group(campaignId).AddComment(comment);
      return Ok(comment);
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
      if (campaignData.PublicNotes != null)
      {
        await _hubContext.Clients.Group(campaignId).CampaignNotes(campaignData.PublicNotes, campaignId);
      }
      if (campaignData.Initiative != null)
      {
        await _hubContext.Clients.Group(campaignId).InitiateBattle(campaignData.Initiative, campaignId);
      }
      return Ok(campaign);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPut("{campaignId}/comments/{commentId}")]
  [Authorize]
  public async Task<ActionResult<Comment>> UpdateCommentByCampaignId([FromBody] Comment commentData, string campaignId, string commentId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      commentData.CreatorId = userInfo.Id;
      commentData.Id = commentId;
      commentData.CampaignId = campaignId;
      Comment comment = _commentsService.UpdateCommentByCampaignId(commentData);
      await _hubContext.Clients.Group(campaignId).UpdateComment(comment);
      return Ok(comment);
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
      await _hubContext.Clients.Group(campaignId).PlayerLeftCampaign(player);
      return Ok(player);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpDelete("{campaignId}/comments/{commentId}")]
  [Authorize]
  public async Task<ActionResult<Comment>> RemoveCommentByCampaignId(string campaignId, string commentId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      Comment comment =
      _campaignsService.RemoveCommentByCampaignId(campaignId, commentId, userInfo.Id);
      await _hubContext.Clients.Group(campaignId).RemoveComment(comment.Id);
      return Ok(comment);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}