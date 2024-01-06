namespace Logitar.Identity.Domain.Settings;

public interface IUserSettingsResolver
{
  IUserSettings Resolve();
}
