namespace Logitar.Identity.Domain.Settings;

public record UserSettings : IUserSettings
{
  public IUniqueNameSettings UniqueName { get; set; } = new UniqueNameSettings();
}
