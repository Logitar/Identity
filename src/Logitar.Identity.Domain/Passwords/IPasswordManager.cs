namespace Logitar.Identity.Domain.Passwords;

public interface IPasswordManager
{
  Password Create(string password);
  Password Decode(string password);
  Password Generate(int length, out byte[] password);
}
