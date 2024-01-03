using FluentValidation;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Infrastructure.Passwords;

public class PasswordManager : IPasswordManager
{
  public PasswordManager(IEnumerable<IPasswordStrategy> strategies, IUserSettings userSettings) // TODO(fpion): not multitenant-compatible
  {
    Strategies = [];
    UserSettings = userSettings;

    foreach (IPasswordStrategy strategy in strategies)
    {
      Strategies[strategy.Key] = strategy;
    }
  }

  protected Dictionary<string, IPasswordStrategy> Strategies { get; }
  protected IUserSettings UserSettings { get; }

  public virtual Password Create(string password)
  {
    IPasswordSettings passwordSettings = UserSettings.PasswordSettings;
    new PasswordValidator(passwordSettings).ValidateAndThrow(password);
    return FindStrategy(passwordSettings.HashingStrategy).Create(password);
  }

  public virtual Password Decode(string password)
  {
    string key = password.Split(Password.Separator).First();
    return FindStrategy(key).Decode(password);
  }

  protected virtual IPasswordStrategy FindStrategy(string key)
  {
    if (!Strategies.TryGetValue(key, out IPasswordStrategy? strategy))
    {
      throw new PasswordStrategyNotSupportedException(key);
    }

    return strategy;
  }
}
