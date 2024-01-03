namespace Logitar.Identity.Domain.Settings;

public interface IUserSettings
{
  IUniqueNameSettings UniqueNameSettings { get; }
  IPasswordSettings PasswordSettings { get; }

  bool RequireConfirmedAccount { get; }
  bool RequireUniqueEmail { get; }
}
