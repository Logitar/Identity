using Logitar.Identity.ApiKeys;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Identity.Demo.Controllers.Api;

[ApiController]
[Route("api/keys")]
public class ApiKeyApiController : ControllerBase
{
  private readonly IApiKeyService _apiKeyService;

  public ApiKeyApiController(IApiKeyService apiKeyService)
  {
    _apiKeyService = apiKeyService;
  }

  [HttpPost]
  public async Task<ActionResult<ApiKey>> CreateAsync([FromBody] CreateApiKeyInput input, CancellationToken cancellationToken)
  {
    input.Prefix = Constants.ApiKeyPrefix;

    ApiKey apiKey = await _apiKeyService.CreateAsync(input, cancellationToken);
    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/keys/{apiKey.Id}");

    return Created(uri, apiKey);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiKey>> DeleteAsync(Guid id, CancellationToken cancellationToken)
  {
    return Ok(await _apiKeyService.DeleteAsync(id, cancellationToken));
  }

  [HttpGet]
  public async Task<ActionResult<PagedList<ApiKey>>> GetAsync(string? realm, string? search,
    ApiKeySort? sort, bool isDescending, int? skip, int? take, CancellationToken cancellationToken)
  {
    return Ok(await _apiKeyService.GetAsync(realm, search, sort, isDescending, skip, take, cancellationToken));
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiKey>> GetAsync(Guid id, CancellationToken cancellationToken)
  {
    ApiKey? apiKey = await _apiKeyService.GetAsync(id, cancellationToken);
    if (apiKey == null)
    {
      return NotFound();
    }

    return Ok(apiKey);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<ApiKey>> UpdateAsync(Guid id, [FromBody] UpdateApiKeyInput input, CancellationToken cancellationToken)
  {
    return Ok(await _apiKeyService.UpdateAsync(id, input, cancellationToken));
  }
}
