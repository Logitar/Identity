using Logitar.Identity.Realms;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Identity.Demo.Controllers.Api;

[ApiController]
[Route("api/realms")]
public class RealmApiController : ControllerBase
{
  private readonly IRealmService _realmService;

  public RealmApiController(IRealmService realmService)
  {
    _realmService = realmService;
  }

  [HttpPost]
  public async Task<ActionResult<Realm>> CreateAsync([FromBody] CreateRealmInput input, CancellationToken cancellationToken)
  {
    Realm realm = await _realmService.CreateAsync(input, cancellationToken);
    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/realms/{realm.Id}");

    return Created(uri, realm);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<Realm>> DeleteAsync(Guid id, CancellationToken cancellationToken)
  {
    return Ok(await _realmService.DeleteAsync(id, cancellationToken));
  }

  [HttpGet]
  public async Task<ActionResult<PagedList<Realm>>> GetAsync(string? search, RealmSort? sort,
    bool isDescending, int? skip, int? take, CancellationToken cancellationToken)
  {
    return Ok(await _realmService.GetAsync(search, sort, isDescending, skip, take, cancellationToken));
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Realm>> GetAsync(Guid id, CancellationToken cancellationToken)
  {
    Realm? realm = await _realmService.GetAsync(id, uniqueName: null, cancellationToken);
    if (realm == null)
    {
      return NotFound();
    }

    return Ok(realm);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Realm>> UpdateAsync(Guid id, [FromBody] UpdateRealmInput input, CancellationToken cancellationToken)
  {
    return Ok(await _realmService.UpdateAsync(id, input, cancellationToken));
  }
}
