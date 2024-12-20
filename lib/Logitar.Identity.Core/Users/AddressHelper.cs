namespace Logitar.Identity.Core.Users;

/// <summary>
/// Implements helper methods for postal addresses.
/// </summary>
public class AddressHelper : IAddressHelper
{
  /// <summary>
  /// The country settings.
  /// </summary>
  protected Dictionary<string, CountrySettings> Countries { get; } = new()
  {
    ["CA"] = new()
    {
      PostalCode = "[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ -]?\\d[ABCEGHJ-NPRSTV-Z]\\d$",
      Regions = ["AB", "BC", "MB", "NB", "NL", "NT", "NS", "NU", "ON", "PE", "QC", "SK", "YT"]
    }
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="AddressHelper"/> class.
  /// </summary>
  /// <param name="countries">The country settings.</param>
  public AddressHelper(IEnumerable<KeyValuePair<string, CountrySettings>>? countries = null)
  {
    if (countries != null)
    {
      foreach (KeyValuePair<string, CountrySettings> country in countries)
      {
        Countries[country.Key] = country.Value;
      }
    }
  }

  /// <summary>
  /// Gets the list of supported countries.
  /// </summary>
  public virtual IReadOnlyCollection<string> SupportedCountries => Countries.Keys;

  /// <summary>
  /// Gets the validation settings of the specified country.
  /// </summary>
  /// <param name="country">The country.</param>
  /// <returns>The validation settings of the specified country, or null if the country is not supported.</returns>
  public virtual CountrySettings? GetCountry(string country) => Countries.TryGetValue(country, out CountrySettings? settings) ? settings : null;

  /// <summary>
  /// Returns a value indicating whether or not the specified country is supported.
  /// </summary>
  /// <param name="country">The country.</param>
  /// <returns>True if the specified country is supported, or false otherwise.</returns>
  public virtual bool IsSupported(string country) => Countries.ContainsKey(country);
}
