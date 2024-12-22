using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Infrastructure.Passwords;

internal class PasswordManagerMock : PasswordManager
{
  public new IUserSettingsResolver SettingsResolver => base.SettingsResolver;
  public new Dictionary<string, IPasswordStrategy> Strategies => base.Strategies;

  public PasswordManagerMock(IUserSettingsResolver settingsResolver, IEnumerable<IPasswordStrategy> strategies)
    : base(settingsResolver, strategies)
  {
  }

  public new IPasswordStrategy GetStrategy(string key) => base.GetStrategy(key);
}

