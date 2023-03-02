using Logitar.Identity.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Identity.Demo.Controllers.Api;

[ApiController]
[Route("api/sessions")]
public class SessionApiController : ControllerBase
{
  private readonly ISessionService _sessionService;

  public SessionApiController(ISessionService sessionService)
  {
    _sessionService = sessionService;
  }

  [HttpGet]
  public async Task<ActionResult<PagedList<Session>>> GetAsync(bool? isActive, bool? isPersistent, string? realm, Guid? userId,
    SessionSort? sort, bool isDescending, int? skip, int? take, CancellationToken cancellationToken)
  {
    return Ok(await _sessionService.GetAsync(isActive, isPersistent, realm, userId,
      sort, isDescending, skip, take, cancellationToken));
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Session>> GetAsync(Guid id, CancellationToken cancellationToken)
  {
    Session? session = await _sessionService.GetAsync(id, cancellationToken);
    if (session == null)
    {
      return NotFound();
    }

    return Ok(session);
  }

  [HttpPost("refresh")]
  public async Task<ActionResult<Session>> RefreshAsync([FromBody] RefreshSessionInput input, CancellationToken cancellationToken)
  {
    return Ok(await _sessionService.RefreshAsync(input, cancellationToken));
  }

  [HttpPost("sign/in")]
  public async Task<ActionResult<Session>> SignInAsync([FromBody] SignInInput input, CancellationToken cancellationToken)
  {
    return Ok(await _sessionService.SignInAsync(input, cancellationToken));
  }

  [HttpPut("{id}/sign/out")]
  public async Task<ActionResult<Session>> SignOutAsync(Guid id, CancellationToken cancellationToken)
  {
    return Ok(await _sessionService.SignOutAsync(id, cancellationToken));
  }

  [HttpPut("sign/out/user/{id}")]
  public async Task<ActionResult<IEnumerable<Session>>> SignOutUserAsync(Guid id, CancellationToken cancellationToken)
  {
    return Ok(await _sessionService.SignOutUserAsync(id, cancellationToken));
  }
}
