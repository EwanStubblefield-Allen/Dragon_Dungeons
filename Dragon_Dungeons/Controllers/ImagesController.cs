namespace Dragon_Dungeons.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController(ImagesService imagesService, Config config) : ControllerBase
{
  private readonly ImagesService _imagesService = imagesService;
  private readonly Config _config = config;

  [HttpPost]
  public ActionResult<object> CreateImage([FromBody] CreateImage createImage)
  {
    try
    {
      createImage.Api_key = _config.IMAGE_API_SECRET;
      object image = _imagesService.ImageRequest(createImage, "upload");
      return Ok(image);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpDelete]
  public ActionResult<object> DeleteImage([FromBody] RemoveImage removeImage)
  {
    try
    {
      removeImage.Signature = _imagesService.ConvertToSha256(removeImage);
      object image = _imagesService.ImageRequest(removeImage, "delete");
      return Ok(image);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}