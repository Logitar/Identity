using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

/// <summary>
/// The validator used to validate user genders.
/// See <see cref="GenderUnit"/> for more information.
/// </summary>
public class GenderValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="GenderValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public GenderValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(GenderUnit.MaximumLength)
      .WithPropertyName(propertyName);
  }
}
