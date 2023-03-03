using FluentValidation;
using Logitar.Identity.Sessions.Events;

namespace Logitar.Identity.Sessions.Validators;

/// <summary>
/// The validator used to validate instances of <see cref="SessionSavedEvent"/> classes.
/// </summary>
/// <typeparam name="T">The type of the inheriting validator.</typeparam>
internal class SessionSavedValidator<T> : AbstractValidator<T> where T : SessionSavedEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionSavedValidator{T}"/> class.
  /// </summary>
  protected SessionSavedValidator()
  {
    RuleFor(x => x.KeyHash).NullOrNotEmpty();

    RuleForEach(x => x.CustomAttributes.Keys).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();
    RuleForEach(x => x.CustomAttributes.Values).NotEmpty();
  }
}
