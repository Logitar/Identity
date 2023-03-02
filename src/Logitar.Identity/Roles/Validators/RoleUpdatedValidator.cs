using Logitar.Identity.Roles.Events;

namespace Logitar.Identity.Roles.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="RoleUpdatedEvent"/> class.
/// </summary>
internal class RoleUpdatedValidator : RoleSavedValidator<RoleUpdatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RoleUpdatedValidator"/> class.
  /// </summary>
  public RoleUpdatedValidator() : base()
  {
  }
}
