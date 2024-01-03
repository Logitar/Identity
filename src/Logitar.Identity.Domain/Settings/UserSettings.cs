namespace Logitar.Identity.Domain.Settings;

public record UserSettings : IUserSettings
{
  public IUniqueNameSettings UniqueNameSettings { get; set; } = new UniqueNameSettings();
  public IPasswordSettings PasswordSettings { get; set; } = new PasswordSettings();

  public bool RequireConfirmedAccount { get; set; }
  public bool RequireUniqueEmail { get; set; }
}
