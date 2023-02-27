using FluentValidation;
using Logitar.Identity.Users.Events;

namespace Logitar.Identity.Users.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="UserChangedPasswordEvent"/> class.
/// </summary>
internal class UserChangedPasswordValidator : AbstractValidator<UserChangedPasswordEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserChangedPasswordValidator"/> class.
  /// </summary>
  public UserChangedPasswordValidator()
  {
    RuleFor(x => x.PasswordHash).NotEmpty();
  }
}
