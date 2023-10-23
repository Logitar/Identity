using System.Collections.Immutable;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents validation settings for a country.
/// </summary>
public record CountrySettings
{
  /// <summary>
  /// Gets or sets the postal code validation regular expression for this country.
  /// </summary>
  public string? PostalCode { get; init; }
  /// <summary>
  /// Gets or sets the allowed regions for this country.
  /// </summary>
  public ImmutableHashSet<string>? Regions { get; init; }
}
