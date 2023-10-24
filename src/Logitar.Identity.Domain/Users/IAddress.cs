namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Describes postal addresses.
/// </summary>
public interface IAddress
{
  /// <summary>
  /// Gets the street address.
  /// </summary>
  string Street { get; }
  /// <summary>
  /// Gets the locality (city) of the address.
  /// </summary>
  string Locality { get; }
  /// <summary>
  /// Gets the postal code of the address.
  /// </summary>
  string? PostalCode { get; }
  /// <summary>
  /// Gets the region of the address.
  /// </summary>
  string? Region { get; }
  /// <summary>
  /// Gets the country of the address.
  /// </summary>
  string Country { get; }
}
