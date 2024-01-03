using FluentValidation;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Infrastructure.Passwords;

public class PasswordManager : IPasswordManager
{
  private readonly Dictionary<string, IPasswordStrategy> _strategies = [];
  private readonly IUserSettings _userSettings;

  public PasswordManager(IEnumerable<IPasswordStrategy> strategies, IUserSettings userSettings) // TODO(fpion): not multitenant-compatible
  {
    foreach (IPasswordStrategy strategy in strategies)
    {
      _strategies[strategy.Key] = strategy;
    }

    _userSettings = userSettings;
  }

  public virtual Password Create(string password)
  {
    IPasswordSettings passwordSettings = _userSettings.PasswordSettings;
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
    if (!_strategies.TryGetValue(key, out IPasswordStrategy? strategy))
    {
      throw new PasswordStrategyNotSupportedException(key);
    }

    return strategy;
  }
}
