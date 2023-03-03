using Logitar.Identity.Users;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Identity.Demo.Controllers.Api;

[ApiController]
[Route("api/users")]
public class UserApiController : ControllerBase
{
  private readonly IUserService _userService;

  public UserApiController(IUserService userService)
  {
    _userService = userService;
  }

  [HttpPut("{id}/password/change")]
  public async Task<ActionResult<User>> ChangePasswordAsync(Guid id, [FromBody] ChangePasswordInput input, CancellationToken cancellationToken)
  {
    return Ok(await _userService.ChangePasswordAsync(id, input, cancellationToken));
  }

  [HttpPost]
  public async Task<ActionResult<User>> CreateAsync([FromBody] CreateUserInput input, CancellationToken cancellationToken)
  {
    User user = await _userService.CreateAsync(input, cancellationToken);
    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/users/{user.Id}");

    return Created(uri, user);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<User>> DeleteAsync(Guid id, CancellationToken cancellationToken)
  {
    return Ok(await _userService.DeleteAsync(id, cancellationToken));
  }

  [HttpDelete("{id}/{externalKey}")]
  public async Task<ActionResult<User>> DeleteAsync(Guid id, string externalKey, CancellationToken cancellationToken)
  {
    return Ok(await _userService.SaveExternalIdentifierAsync(id, externalKey, value: null, cancellationToken));
  }

  [HttpGet]
  public async Task<ActionResult<PagedList<User>>> GetAsync(bool? isConfirmed, bool? isDisabled, string? realm, string? search,
    UserSort? sort, bool isDescending, int? skip, int? take, CancellationToken cancellationToken)
  {
    return Ok(await _userService.GetAsync(isConfirmed, isDisabled, realm, search, sort, isDescending, skip, take, cancellationToken));
  }

  [HttpGet("{realm}/{username}")]
  public async Task<ActionResult<User>> GetAsync(string realm, string username, CancellationToken cancellationToken)
  {
    User? user = await _userService.GetAsync(realm: realm, username: username, cancellationToken: cancellationToken);
    if (user == null)
    {
      return NotFound();
    }

    return Ok(user);
  }

  [HttpGet("{realm}/{externalKey}/{externalValue}")]
  public async Task<ActionResult<User>> GetAsync(string realm, string externalKey, string externalValue, CancellationToken cancellationToken)
  {
    User? user = await _userService.GetAsync(realm: realm, externalKey: externalKey, externalValue: externalValue, cancellationToken: cancellationToken);
    if (user == null)
    {
      return NotFound();
    }

    return Ok(user);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetAsync(Guid id, CancellationToken cancellationToken)
  {
    User? user = await _userService.GetAsync(id, cancellationToken: cancellationToken);
    if (user == null)
    {
      return NotFound();
    }

    return Ok(user);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<User>> UpdateAsync(Guid id, [FromBody] UpdateUserInput input, CancellationToken cancellationToken)
  {
    return Ok(await _userService.UpdateAsync(id, input, cancellationToken));
  }

  [HttpPatch("{id}/disable")]
  public async Task<ActionResult<User>> DisableAsync(Guid id, CancellationToken cancellationToken)
  {
    return Ok(await _userService.DisableAsync(id, cancellationToken));
  }

  [HttpPatch("{id}/enable")]
  public async Task<ActionResult<User>> EnableAsync(Guid id, CancellationToken cancellationToken)
  {
    return Ok(await _userService.EnableAsync(id, cancellationToken));
  }

  [HttpPatch("{id}/{externalKey}/{externalValue}")]
  public async Task<ActionResult<User>> SaveAsync(Guid id, string externalKey, string externalValue, CancellationToken cancellationToken)
  {
    return Ok(await _userService.SaveExternalIdentifierAsync(id, externalKey, externalValue, cancellationToken));
  }
}
