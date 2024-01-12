using FluentValidation;
using Logitar.Identity.Domain.Users.Validators;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents a postal address.
/// </summary>
public record AddressUnit : ContactUnit, IAddress
{
  /// <summary>
  /// The maximum length of address components.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// Gets the street address.
  /// </summary>
  public string Street { get; }
  /// <summary>
  /// Gets the locality (city) of the address.
  /// </summary>
  public string Locality { get; }
  /// <summary>
  /// Gets the postal code of the address.
  /// </summary>
  public string? PostalCode { get; }
  /// <summary>
  /// Gets the region of the address.
  /// </summary>
  public string? Region { get; }
  /// <summary>
  /// Gets the country of the address.
  /// </summary>
  public string Country { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="AddressUnit"/> class.
  /// </summary>
  /// <param name="street">The street address.</param>
  /// <param name="locality">The locality (city) of the address.</param>
  /// <param name="country">The country of the address.</param>
  /// <param name="region">The region of the address.</param>
  /// <param name="postalCode">The postal code of the address.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public AddressUnit(string street, string locality, string country, string? region = null, string? postalCode = null, string? propertyName = null)
    : this(street, locality, country, region, postalCode, isVerified: false, propertyName)
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="AddressUnit"/> class.
  /// </summary>
  /// <param name="street">The street address.</param>
  /// <param name="locality">The locality (city) of the address.</param>
  /// <param name="country">The country of the address.</param>
  /// <param name="region">The region of the address.</param>
  /// <param name="postalCode">The postal code of the address.</param>
  /// <param name="isVerified">A value indicating whether or not the postal address is verified.</param>
  [JsonConstructor]
  public AddressUnit(string street, string locality, string country, string? region, string? postalCode, bool isVerified)
    : this(street, locality, country, region, postalCode, isVerified, propertyName: null)
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="AddressUnit"/> class.
  /// </summary>
  /// <param name="street">The street address.</param>
  /// <param name="locality">The locality (city) of the address.</param>
  /// <param name="country">The country of the address.</param>
  /// <param name="region">The region of the address.</param>
  /// <param name="postalCode">The postal code of the address.</param>
  /// <param name="isVerified">A value indicating whether or not the postal address is verified.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public AddressUnit(string street, string locality, string country, string? region, string? postalCode, bool isVerified, string? propertyName = null)
    : base(isVerified)
  {
    Street = street.Trim();
    Locality = locality.Trim();
    PostalCode = postalCode?.CleanTrim();
    Region = region?.CleanTrim();
    Country = country.Trim();

    new AddressValidator(propertyName).ValidateAndThrow(this);
  }

  /// <summary>
  /// Formats the postal address.
  /// </summary>
  /// <returns></returns>
  public string Format()
  {
    StringBuilder formatted = new();

    string[] lines = Street.Remove("\r").Split('\n');
    foreach (string line in lines)
    {
      if (!string.IsNullOrWhiteSpace(line))
      {
        formatted.AppendLine(line.Trim());
      }
    }

    formatted.Append(Locality);
    if (Region != null)
    {
      formatted.Append(' ').Append(Region);
    }
    if (PostalCode != null)
    {
      formatted.Append(' ').Append(PostalCode);
    }
    formatted.AppendLine();

    formatted.Append(Country);

    return formatted.ToString();
  }
}
