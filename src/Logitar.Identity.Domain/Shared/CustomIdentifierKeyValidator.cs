using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate custom identifier keys.
/// See <see cref="CustomIdentifierValidator"/> for more information.
/// </summary>
public class CustomIdentifierKeyValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CustomIdentifierKeyValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public CustomIdentifierKeyValidator(string? propertyName = null)
  {
    RuleFor(x => x).SetValidator(new IdentifierValidator()).WithPropertyName(propertyName);
  }
}
