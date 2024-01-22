using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain;

internal record PasswordMock : Password // TODO(fpion): replace this class by the Base64Password
{
  private readonly string _password;

  public PasswordMock(string password)
  {
    _password = password;
  }

  public override string Encode() => Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_password));

  public override bool IsMatch(string password) => _password == password;
}
