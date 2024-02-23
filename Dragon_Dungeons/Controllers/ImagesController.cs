namespace Dragon_Dungeons.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController(ImagesService imagesService, Config config) : ControllerBase
{
  private readonly ImagesService _imagesService = imagesService;
  private readonly Config _config = config;

  [HttpPost("generations")]
  public ActionResult<string> GenerateImage([FromBody] object prompt)
  {
    try
    {
      string image = _imagesService.GenerateImage(prompt.ToString());
      return Ok(image);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPost("creation")]
  public ActionResult<string> CreateImage([FromBody] CreateImage createImage)
  {
    try
    {
      createImage.Api_key = _config.IMAGE_API_SECRET;
      string image = _imagesService.ImageRequest(createImage, "upload");
      return Ok(image);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPost("deletion")]
  public ActionResult<string> DeleteImage([FromBody] RemoveImage removeImage)
  {
    try
    {
      removeImage.Api_Key = _config.IMAGE_API_KEY;
      removeImage.Signature = _imagesService.ConvertToSha256(removeImage);
      string image = _imagesService.ImageRequest(removeImage, "destroy");
      return Ok(image);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}