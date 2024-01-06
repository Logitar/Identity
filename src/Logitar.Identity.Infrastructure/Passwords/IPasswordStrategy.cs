using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Infrastructure.Passwords;

public interface IPasswordStrategy
{
  string Key { get; }

  Password Create(string password);
  Password Decode(string password);
}
