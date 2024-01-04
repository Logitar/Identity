using Logitar.Identity.Application.Account;
using Logitar.Identity.Contracts.Account;
using Logitar.Identity.Contracts.Sessions;
using Logitar.Identity.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Identity.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
  private readonly IAccountService _accountService;

  public AccountController(IAccountService accountService)
  {
    _accountService = accountService;
  }

  [HttpPost("register")]
  public async Task<ActionResult> RegisterAsync([FromBody] RegisterPayload payload, CancellationToken cancellationToken)
  {
    await _accountService.RegisterAsync(payload, cancellationToken);
    return NoContent();
  }

  [HttpPost("sign/in")]
  public async Task<ActionResult> SignInAsync([FromBody] SignInPayload payload, CancellationToken cancellationToken)
  {
    Session session = await _accountService.SignInAsync(payload, cancellationToken);
    HttpContext.SignIn(session);

    return NoContent();
  }
}
