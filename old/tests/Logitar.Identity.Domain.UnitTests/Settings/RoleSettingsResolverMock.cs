using Microsoft.Extensions.Configuration;

namespace Logitar.Identity.Domain.Settings;

internal class RoleSettingsResolverMock : RoleSettingsResolver
{
  private readonly IConfiguration _configuration;

  protected override IConfiguration Configuration
  {
    get
    {
      ReadCounter++;
      return _configuration;
    }
  }

  public int ReadCounter { get; private set; } = 0;

  public RoleSettingsResolverMock(IConfiguration configuration) : base(configuration)
  {
    _configuration = configuration;
  }
}
