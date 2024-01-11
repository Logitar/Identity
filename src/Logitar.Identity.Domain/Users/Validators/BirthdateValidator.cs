using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

/// <summary>
/// The validator used to validate user birth dates.
/// </summary>
public class BirthdateValidator : AbstractValidator<DateTime>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="BirthdateValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public BirthdateValidator(string? propertyName = null)
  {
    RuleFor(x => x).Past().WithPropertyName(propertyName);
  }
}
