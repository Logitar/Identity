namespace Logitar.Identity.Domain.Settings;

public interface IUserSettings
{
  IUniqueNameSettings UniqueName { get; }
  IPasswordSettings Password { get; }

  bool RequireUniqueEmail { get; }
}
