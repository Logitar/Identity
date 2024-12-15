using FluentValidation;

namespace Logitar.Identity.Domain.Passwords.Validators;

/// <summary>
/// The validator used to validate One-Time Password (OTP) maximum attempts.
/// </summary>
public class MaximumAttemptsValidator : AbstractValidator<int>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="MaximumAttemptsValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public MaximumAttemptsValidator(string? propertyName = null)
  {
    RuleFor(x => x).GreaterThan(0)
      .WithErrorCode(nameof(MaximumAttemptsValidator))
      .WithPropertyName(propertyName);
  }
}
