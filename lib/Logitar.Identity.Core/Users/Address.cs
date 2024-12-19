﻿using FluentValidation;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents a postal address.
/// </summary>
public record Address : Contact
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
  /// Initializes a new instance of the <see cref="Address"/> class.
  /// </summary>
  /// <param name="street">The street address.</param>
  /// <param name="locality">The locality (city) of the address.</param>
  /// <param name="country">The country of the address.</param>
  /// <param name="postalCode">The postal code of the address.</param>
  /// <param name="region">The region of the address.</param>
  /// <param name="isVerified">A value indicating whether the contact is verified or not.</param>
  public Address(string street, string locality, string country, string? postalCode = null, string? region = null, bool isVerified = false) : base(isVerified)
  {
    Street = street.Trim();
    Locality = locality.Trim();
    PostalCode = postalCode?.CleanTrim();
    Region = region?.CleanTrim();
    Country = country.Trim();
  }

  /// <summary>
  /// Returns a string representation of the address.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString()
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

  /// <summary>
  /// Represents the validator for instances of <see cref="Address"/>.
  /// </summary>
  private class Validator : AbstractValidator<Address>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Street).NotEmpty().MaximumLength(MaximumLength);
      RuleFor(x => x.Locality).NotEmpty().MaximumLength(MaximumLength);
      When(x => x.PostalCode != null, () => RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(MaximumLength)); // TODO(fpion): validate from CountrySettings
      When(x => x.Region != null, () => RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(MaximumLength)); // TODO(fpion): validate from CountrySettings
      RuleFor(x => x.Country).NotEmpty().MaximumLength(MaximumLength); // TODO(fpion): validate from CountrySettings
    }
  }
}
