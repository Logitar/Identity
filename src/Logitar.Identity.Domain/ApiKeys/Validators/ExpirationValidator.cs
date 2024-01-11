using FluentValidation;

namespace Logitar.Identity.Domain.ApiKeys.Validators;

/// <summary>
/// The validator used to validate API key expirations.
/// </summary>
public class ExpirationValidator : AbstractValidator<DateTime>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ExpirationValidator"/> class.
  /// </summary>
  /// <param name="expiresOn">The current expiration date and time.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public ExpirationValidator(DateTime? expiresOn, string? propertyName = null)
  {
    IRuleBuilderOptions<DateTime, DateTime> options = RuleFor(x => x).NotNull()
      .Future();

    if (expiresOn.HasValue)
    {
      options.LessThanOrEqualTo(expiresOn.Value);
    }

    options.WithPropertyName(propertyName);
  }
}
