using FluentValidation;
using Logitar.Identity.Roles.Events;

namespace Logitar.Identity.Roles.Validators;

/// <summary>
/// The validator used to validate instances of <see cref="RoleSavedEvent"/> classes.
/// </summary>
/// <typeparam name="T">The type of the inheriting validator.</typeparam>
internal abstract class RoleSavedValidator<T> : AbstractValidator<T> where T : RoleSavedEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RoleSavedValidator{T}"/> class.
  /// </summary>
  protected RoleSavedValidator()
  {
    RuleFor(x => x.DisplayName).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Description).NullOrNotEmpty();

    RuleForEach(x => x.CustomAttributes.Keys).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();
    RuleForEach(x => x.CustomAttributes.Values).NotEmpty();
  }
}
