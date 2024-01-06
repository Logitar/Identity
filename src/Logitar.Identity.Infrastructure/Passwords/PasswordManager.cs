using FluentValidation;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Infrastructure.Passwords;

public class PasswordManager : IPasswordManager
{
  public PasswordManager(IUserSettings userSettings, IEnumerable<IPasswordStrategy> strategies) // TODO(fpion): not multitenant
  {
    Settings = userSettings.Password;

    foreach (IPasswordStrategy strategy in strategies)
    {
      Strategies[strategy.Key] = strategy;
    }
  }

  protected IPasswordSettings Settings { get; }
  protected Dictionary<string, IPasswordStrategy> Strategies { get; } = [];

  public virtual Password Create(string password)
  {
    new PasswordValidator(Settings).ValidateAndThrow(password);
    return GetStrategy(Settings.HashingStrategy).Create(password);
  }

  public virtual Password Decode(string password)
  {
    string key = password.Split(Password.Separator).First();
    return GetStrategy(key).Decode(password);
  }

  public virtual Password Generate(int length, out byte[] password)
  {
    password = RandomNumberGenerator.GetBytes(length);
    return GetStrategy(Settings.HashingStrategy).Create(Convert.ToBase64String(password));
  }

  protected virtual IPasswordStrategy GetStrategy(string key)
  {
    return Strategies.TryGetValue(key, out IPasswordStrategy? strategy)
      ? strategy
      : throw new PasswordStrategyNotSupportedException(key);
  }
}
