using Logitar.Identity.Models.Index;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Identity.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("")]
public class IndexController : ControllerBase
{
  [HttpGet]
  public ActionResult<ApiVersion> Get() => Ok(new ApiVersion());
}
