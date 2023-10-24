using NodaTime;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Represents a time zone, from the <see href="https://en.wikipedia.org/wiki/Tz_database">tz database</see>.
/// </summary>
public record TimeZoneUnit
{
  /// <summary>
  /// Gets the identifier of the time zone entry.
  /// </summary>
  public string Id => TimeZone.Id;
  /// <summary>
  /// Gets the time zone entry object.
  /// </summary>
  public DateTimeZone TimeZone { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="TimeZoneUnit"/> class.
  /// </summary>
  /// <param name="timeZone">The time zone entry object.</param>
  public TimeZoneUnit(DateTimeZone timeZone)
  {
    TimeZone = timeZone;
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="TimeZoneUnit"/> class.
  /// </summary>
  /// <param name="id">The identifier of the time zone entry.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public TimeZoneUnit(string id, string? propertyName = null)
  {
    TimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(id.Trim()) ?? throw new InvalidTimeZoneEntryException(id, propertyName);
  }

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="TimeZoneUnit"/> class otherwise.
  /// </summary>
  /// <param name="id">The identifier of the time zone entry.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static TimeZoneUnit? TryCreate(string? id, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(id) ? null : new(id.Trim(), propertyName);
  }
}
