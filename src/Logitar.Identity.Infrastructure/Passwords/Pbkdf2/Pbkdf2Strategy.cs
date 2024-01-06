using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Infrastructure.Passwords.Pbkdf2;

public class Pbkdf2Strategy : IPasswordStrategy
{
  public string Key => Pbkdf2.Key;

  private readonly Pbkdf2Settings _settings;

  public Pbkdf2Strategy(Pbkdf2Settings settings)
  {
    _settings = settings;
  }

  public Password Create(string password)
  {
    return new Pbkdf2(password, _settings.Algorithm, _settings.Iterations, _settings.SaltLength, _settings.HashLength);
  }

  public Password Decode(string password)
  {
    return Pbkdf2.Decode(password);
  }
}
