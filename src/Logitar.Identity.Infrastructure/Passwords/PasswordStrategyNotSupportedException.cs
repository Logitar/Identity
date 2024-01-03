namespace Logitar.Identity.Infrastructure.Passwords;

public class PasswordStrategyNotSupportedException : NotSupportedException
{
  public string Key
  {
    get => (string)Data[nameof(Key)]!;
    private set => Data[nameof(Key)] = value;
  }

  public PasswordStrategyNotSupportedException(string key) : base($"The password strategy '{key}' is not supported.")
  {
    Key = key;
  }
}
