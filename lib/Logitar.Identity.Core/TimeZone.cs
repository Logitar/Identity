using FluentValidation;
using NodaTime;

namespace Logitar.Identity.Core;

/// <summary>
/// Represents a time zone, from the <see href="https://en.wikipedia.org/wiki/Tz_database">tz database</see>.
/// </summary>
public record TimeZone
{
  /// <summary>
  /// The maximum length of time zone identifiers.
  /// </summary>
  public const int MaximumLength = 32;

  /// <summary>
  /// Gets the date time zone entry object.
  /// </summary>
  public DateTimeZone DateTimeZone { get; }
  /// <summary>
  /// Gets the identifier of the time zone entry.
  /// </summary>
  public string Id { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="TimeZone"/> class.
  /// </summary>
  /// <param name="dateTimeZone">The date time zone entry object.</param>
  public TimeZone(DateTimeZone dateTimeZone)
  {
    Id = dateTimeZone.Id;
    new Validator().ValidateAndThrow(this);

    DateTimeZone = dateTimeZone;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="TimeZone"/> class.
  /// </summary>
  /// <param name="id">The identifier of the time zone entry.</param>
  /// <exception cref="InvalidOperationException">The validation succeeded, but the DateTimeZone could not be resolved. This should never happen when validation succeeds.</exception>
  public TimeZone(string id)
  {
    Id = id.Trim();
    new Validator().ValidateAndThrow(this);

    DateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(Id) ?? throw new InvalidOperationException($"The date time zone 'Id={Id}' could not be resolved.");
  }

  /// <summary>
  /// Returns a new instance of the <see cref="TimeZone"/> class, or null if the value is null, empty or only white-space.
  /// </summary>
  /// <param name="value">The string value.</param>
  /// <returns>The new instance, or null.</returns>
  public static TimeZone? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  /// <summary>
  /// Returns a string representation of the time zone.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Id;

  /// <summary>
  /// Represents the validator for instances of <see cref="TimeZone"/>.
  /// </summary>
  private class Validator : AbstractValidator<TimeZone>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Id).TimeZone();
    }
  }
}
