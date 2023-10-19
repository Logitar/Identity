using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate custom attribute values.
/// See <see cref="CustomAttributeValidator"/> for more information.
/// </summary>
public class CustomAttributeValueValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CustomAttributeValueValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public CustomAttributeValueValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .WithPropertyName(propertyName);
  }
}
