using Logitar.Identity.Domain.Passwords;
using Microsoft.Extensions.Configuration;

namespace Logitar.Identity.Infrastructure.Passwords.Pbkdf2;

public class Pbkdf2Strategy : IPasswordStrategy
{
  public string Key => Pbkdf2.Key;

  private readonly Pbkdf2Settings _settings;

  public Pbkdf2Strategy(IConfiguration configuration)
  {
    _settings = configuration.GetSection("Pbkdf2").Get<Pbkdf2Settings>() ?? new();
  }

  public virtual Password Create(string password) => new Pbkdf2(password, _settings.Algorithm, _settings.Iterations, _settings.SaltLength, _settings.HashLength);

  public virtual Password Decode(string password) => Pbkdf2.Decode(password);
}
