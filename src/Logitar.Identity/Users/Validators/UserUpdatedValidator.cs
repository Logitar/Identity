using Logitar.Identity.Users.Events;

namespace Logitar.Identity.Users.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="UserUpdatedEvent"/> class.
/// </summary>
internal class UserUpdatedValidator : UserSavedValidator<UserUpdatedEvent>
{
}
