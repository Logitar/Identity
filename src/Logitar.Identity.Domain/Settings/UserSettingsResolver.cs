using Microsoft.Extensions.Configuration;

namespace Logitar.Identity.Domain.Settings;

public class UserSettingsResolver : IUserSettingsResolver
{
  private readonly IConfiguration _configuration;

  public UserSettingsResolver(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  protected IUserSettings? UserSettings { get; set; }

  public virtual IUserSettings Resolve()
  {
    UserSettings ??= _configuration.GetSection("User").Get<UserSettings>() ?? new();
    return UserSettings;
  }
}
