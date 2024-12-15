using Logitar.Identity.Demo.Models.Index;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Identity.Demo.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("")]
public class IndexController : ControllerBase
{
  [HttpGet]
  public ActionResult<ApiVersion> Get() => Ok(new ApiVersion());
}
