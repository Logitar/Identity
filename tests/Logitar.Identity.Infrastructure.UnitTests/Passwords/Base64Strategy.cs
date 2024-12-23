using Logitar.Identity.Core.Passwords;

namespace Logitar.Identity.Infrastructure.Passwords;

internal class Base64Strategy : IPasswordStrategy
{
  public string Key => Base64Password.Key;

  public Password Create(string password) => new Base64Password(password);

  public Password Decode(string password) => Base64Password.Decode(password);
}
