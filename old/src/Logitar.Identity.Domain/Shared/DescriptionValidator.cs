using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate descriptions.
/// See <see cref="DescriptionUnit"/> for more information.
/// </summary>
public class DescriptionValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DescriptionValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public DescriptionValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty().WithPropertyName(propertyName);
  }
}
