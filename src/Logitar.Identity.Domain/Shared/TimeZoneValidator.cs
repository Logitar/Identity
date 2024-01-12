using FluentValidation;
using NodaTime;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate locales.
/// See <see cref="TimeZoneUnit"/> for more information.
/// </summary>
public class TimeZoneValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="TimeZoneValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public TimeZoneValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(TimeZoneUnit.MaximumLength)
      .Must(id => DateTimeZoneProviders.Tzdb.GetZoneOrNull(id) != null)
        .WithErrorCode(nameof(TimeZoneValidator))
        .WithMessage("'{PropertyName}' did not resolve to a tz entry.")
      .WithPropertyName(propertyName);
  }
}
