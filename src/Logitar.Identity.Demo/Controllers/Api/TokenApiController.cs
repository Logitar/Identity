using Logitar.Identity.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Identity.Demo.Controllers.Api;

[ApiController]
[Route("api/tokens")]
public class TokenApiController : ControllerBase
{
  private readonly ITokenService _tokenService;

  public TokenApiController(ITokenService tokenService)
  {
    _tokenService = tokenService;
  }

  [HttpPost]
  public async Task<ActionResult<string>> CreateAsync([FromBody] CreateTokenInput input, CancellationToken cancellationToken)
  {
    return Ok(await _tokenService.CreateAsync(input, cancellationToken));
  }

  [HttpPost("validate")]
  public async Task<ActionResult<IEnumerable<Claim>>> ValidateAsync(bool consume, [FromBody] ValidateTokenInput input, CancellationToken cancellationToken)
  {
    return Ok(await _tokenService.ValidateAsync(input, consume, cancellationToken));
  }
}
