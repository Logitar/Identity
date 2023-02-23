using Logitar.Identity.Roles;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Identity.Demo.Controllers.Api;

[ApiController]
[Route("api/roles")]
public class RoleApiController : ControllerBase
{
  private readonly IRoleService _roleService;

  public RoleApiController(IRoleService roleService)
  {
    _roleService = roleService;
  }

  [HttpPost]
  public async Task<ActionResult<Role>> CreateAsync([FromBody] CreateRoleInput input, CancellationToken cancellationToken)
  {
    Role role = await _roleService.CreateAsync(input, cancellationToken);
    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/roles/{role.Id}");

    return Created(uri, role);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<Role>> DeleteAsync(Guid id, CancellationToken cancellationToken)
  {
    return Ok(await _roleService.DeleteAsync(id, cancellationToken));
  }

  [HttpGet]
  public async Task<ActionResult<PagedList<Role>>> GetAsync(string? realm, string? search,
    RoleSort? sort, bool isDescending, int? skip, int? take, CancellationToken cancellationToken)
  {
    return Ok(await _roleService.GetAsync(realm, search, sort, isDescending, skip, take, cancellationToken));
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Role>> GetAsync(Guid id, CancellationToken cancellationToken)
  {
    Role? role = await _roleService.GetAsync(id, realm: null, uniqueName: null, cancellationToken);
    if (role == null)
    {
      return NotFound();
    }

    return Ok(role);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Role>> UpdateAsync(Guid id, [FromBody] UpdateRoleInput input, CancellationToken cancellationToken)
  {
    return Ok(await _roleService.UpdateAsync(id, input, cancellationToken));
  }
}
