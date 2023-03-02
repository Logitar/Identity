using FluentValidation;
using Logitar.Identity.Roles.Events;

namespace Logitar.Identity.Roles.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="RoleCreatedEvent"/> class.
/// </summary>
internal class RoleCreatedValidator : RoleSavedValidator<RoleCreatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RoleCreatedValidator"/> class.
  /// </summary>
  public RoleCreatedValidator() : base()
  {
    RuleFor(x => x.UniqueName).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();
  }
}
