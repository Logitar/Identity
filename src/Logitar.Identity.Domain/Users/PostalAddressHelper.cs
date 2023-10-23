using System.Collections.Immutable;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Defines methods to help handle postal addresses.
/// </summary>
public static class PostalAddressHelper
{
  private static readonly Dictionary<string, CountrySettings> _settings = new()
  {
    ["CA"] = new()
    {
      PostalCode = "[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ -]?\\d[ABCEGHJ-NPRSTV-Z]\\d$",
      Regions = ImmutableHashSet.Create("AB", "BC", "MB", "NB", "NL", "NT", "NS", "NU", "ON", "PE", "QC", "SK", "YT")
    }
  };

  /// <summary>
  /// Gets the list of supported countries.
  /// </summary>
  public static IReadOnlyCollection<string> SupportedCountries => _settings.Keys.ToList().AsReadOnly();

  /// <summary>
  /// Gets the validation settings of the specified country.
  /// </summary>
  /// <param name="country">The country.</param>
  /// <returns>The validation settings of the specified country, or null if the country is not supported.</returns>
  public static CountrySettings? GetCountry(string country)
  {
    return _settings.TryGetValue(country, out CountrySettings? settings) ? settings : null;
  }

  /// <summary>
  /// Returns a value indicating whether or not the specified country is supported.
  /// </summary>
  /// <param name="country">The country.</param>
  /// <returns>True if the specified country is supported, or false otherwise.</returns>
  public static bool IsSupported(string country) => _settings.ContainsKey(country);
}
