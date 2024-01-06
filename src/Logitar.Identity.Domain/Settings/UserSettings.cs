namespace Logitar.Identity.Domain.Settings;

public record UserSettings : IUserSettings
{
  public IUniqueNameSettings UniqueName { get; set; } = new UniqueNameSettings();
  public IPasswordSettings Password { get; set; } = new PasswordSettings();

  public bool RequireUniqueEmail { get; set; } = false;
}
