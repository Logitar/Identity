using Logitar.Identity.Users.Events;

namespace Logitar.Identity.Users.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="UserUpdatedEvent"/> class.
/// </summary>
internal class UserUpdatedValidator : UserSavedValidator<UserUpdatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserUpdatedValidator"/> class.
  /// </summary>
  public UserUpdatedValidator() : base()
  {
  }
}
