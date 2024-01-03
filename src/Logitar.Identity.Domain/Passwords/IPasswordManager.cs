namespace Logitar.Identity.Domain.Passwords;

public interface IPasswordManager
{
  Password Create(string password);
  Password Decode(string password);
}
