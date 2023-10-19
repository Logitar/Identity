using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate display names.
/// See <see cref="DisplayNameUnit"/> for more information.
/// </summary>
public class DisplayNameValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DisplayNameValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public DisplayNameValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(DisplayNameUnit.MaximumLength)
      .WithPropertyName(propertyName);
  }
}
