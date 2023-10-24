using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain.Users;

internal class PasswordMock : Password
{
  private readonly string _password;

  public PasswordMock(string password)
  {
    _password = password;
  }

  public override string Encode() => Convert.ToBase64String(Encoding.UTF8.GetBytes(_password));

  public override bool IsMatch(string password) => _password == password;
}
