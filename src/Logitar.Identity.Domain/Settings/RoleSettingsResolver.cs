using Microsoft.Extensions.Configuration;

namespace Logitar.Identity.Domain.Settings;

public class RoleSettingsResolver : IRoleSettingsResolver
{
  private readonly IConfiguration _configuration;

  public RoleSettingsResolver(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  protected IRoleSettings? RoleSettings { get; set; }

  public virtual IRoleSettings Resolve()
  {
    RoleSettings ??= _configuration.GetSection("Role").Get<RoleSettings>() ?? new();
    return RoleSettings;
  }
}
