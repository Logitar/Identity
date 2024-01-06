using FluentValidation;
using NodaTime;

namespace Logitar.Identity.Domain.Shared;

public record TimeZoneUnit
{
  public const int MaximumLength = 32;

  public DateTimeZone TimeZone { get; }
  public string Id => TimeZone.Id;

  public TimeZoneUnit(string id)
  {
    id = id.Trim();
    new TimeZoneValidator().ValidateAndThrow(id);

    TimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(id)
      ?? throw new InvalidOperationException($"The time zone '{id}' could not be found. This is likely to be a {nameof(TimeZoneValidator)} failure.");
  }

  public static TimeZoneUnit? TryCreate(string? id) => string.IsNullOrWhiteSpace(id) ? null : new(id);
}
