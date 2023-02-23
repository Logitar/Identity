using FluentValidation;
using Logitar.Identity.ApiKeys.Events;

namespace Logitar.Identity.ApiKeys.Validators;

/// <summary>
/// The validator used to validate instances of <see cref="ApiKeySavedEvent"/> classes.
/// </summary>
/// <typeparam name="T">The type of the inheriting validator.</typeparam>
internal abstract class ApiKeySavedValidator<T> : AbstractValidator<T> where T : ApiKeySavedEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeySavedValidator{T}"/> class.
  /// </summary>
  public ApiKeySavedValidator()
  {
    RuleFor(x => x.Title).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Description).NullOrNotEmpty();

    RuleForEach(x => x.CustomAttributes.Keys).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();
    RuleForEach(x => x.CustomAttributes.Values).NotEmpty();
  }
}
