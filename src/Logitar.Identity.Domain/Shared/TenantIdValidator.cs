using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate tenant identifiers.
/// See <see cref="TenantId"/> for more information.
/// </summary>
public class TenantIdValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="TenantId"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public TenantIdValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(TenantId.MaximumLength)
      .WithPropertyName(propertyName);
  }
}
