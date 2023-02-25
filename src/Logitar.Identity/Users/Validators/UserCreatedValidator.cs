using FluentValidation;
using Logitar.Identity.Realms;
using Logitar.Identity.Users.Events;

namespace Logitar.Identity.Users.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="UserCreatedEvent"/> class.
/// </summary>
internal class UserCreatedValidator : UserSavedValidator<UserCreatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserCreatedValidator"/> class using the specified arguments.
  /// </summary>
  /// <param name="usernameSettings">The settings used to validate usernames.</param>
  public UserCreatedValidator(ReadOnlyUsernameSettings usernameSettings) : base()
  {
    RuleFor(x => x.Username).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Username(usernameSettings);
  }
}
