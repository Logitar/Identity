namespace Logitar.Identity.Core.Users;

/// <summary>
/// Defines helper methods for postal addresses.
/// </summary>
public interface IAddressHelper
{
  /// <summary>
  /// Gets the list of supported countries.
  /// </summary>
  IReadOnlyCollection<string> SupportedCountries { get; }

  /// <summary>
  /// Gets the validation settings of the specified country.
  /// </summary>
  /// <param name="country">The country.</param>
  /// <returns>The validation settings of the specified country, or null if the country is not supported.</returns>
  CountrySettings? GetCountry(string country);

  /// <summary>
  /// Returns a value indicating whether or not the specified country is supported.
  /// </summary>
  /// <param name="country">The country.</param>
  /// <returns>True if the specified country is supported, or false otherwise.</returns>
  bool IsSupported(string country);
}
