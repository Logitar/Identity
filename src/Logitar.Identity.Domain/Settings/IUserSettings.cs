namespace Logitar.Identity.Domain.Settings;

public interface IUserSettings
{
  IUniqueNameSettings UniqueName { get; }
}
