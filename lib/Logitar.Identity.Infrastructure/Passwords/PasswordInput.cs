using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Core;

namespace Logitar.Identity.Infrastructure.Passwords;

internal record PasswordInput
{
  public string Password { get; }

  public PasswordInput(string password, IPasswordSettings passwordSettings)
  {
    Password = password;
    new Validator(passwordSettings).ValidateAndThrow(this);
  }

  private class Validator : AbstractValidator<PasswordInput>
  {
    public Validator(IPasswordSettings passwordSettings)
    {
      RuleFor(x => x.Password).Password(passwordSettings);
    }
  }
}
