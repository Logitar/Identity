using NodaTime;

namespace Logitar.Identity.Domain.Shared;

public record TimeZoneUnit
{
  public const int MaximumLength = 32;

  public DateTimeZone TimeZone { get; }
  public string Id => TimeZone.Id;

  public TimeZoneUnit(string id)
  {
    TimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(id.Trim()) ?? throw new InvalidTimeZoneEntryException(id);
  }

  public static TimeZoneUnit? TryCreate(string? id) => string.IsNullOrWhiteSpace(id) ? null : new(id);
}
