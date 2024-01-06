using FluentValidation;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Infrastructure.Passwords;

public class PasswordManager : IPasswordManager
{
  public PasswordManager(IUserSettingsResolver settingsResolver, IEnumerable<IPasswordStrategy> strategies)
  {
    SettingsResolver = settingsResolver;

    foreach (IPasswordStrategy strategy in strategies)
    {
      Strategies[strategy.Key] = strategy;
    }
  }

  protected IUserSettingsResolver SettingsResolver { get; }
  protected Dictionary<string, IPasswordStrategy> Strategies { get; } = [];

  public virtual Password Create(string password)
  {
    IPasswordSettings passwordSettings = SettingsResolver.Resolve().Password;
    new PasswordValidator(passwordSettings).ValidateAndThrow(password);

    return GetStrategy(passwordSettings.HashingStrategy).Create(password);
  }

  public virtual Password Decode(string password)
  {
    string key = password.Split(Password.Separator).First();
    return GetStrategy(key).Decode(password);
  }

  public virtual Password Generate(int length, out byte[] password)
  {
    password = RandomNumberGenerator.GetBytes(length);

    IPasswordSettings passwordSettings = SettingsResolver.Resolve().Password;
    return GetStrategy(passwordSettings.HashingStrategy).Create(Convert.ToBase64String(password));
  }

  protected virtual IPasswordStrategy GetStrategy(string key)
  {
    return Strategies.TryGetValue(key, out IPasswordStrategy? strategy)
      ? strategy
      : throw new PasswordStrategyNotSupportedException(key);
  }
}
