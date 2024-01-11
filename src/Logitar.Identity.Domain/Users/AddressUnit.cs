using FluentValidation;
using Logitar.Identity.Domain.Users.Validators;
using System.Text;

namespace Logitar.Identity.Domain.Users;

public record AddressUnit : ContactUnit, IAddress
{
  public const int MaximumLength = byte.MaxValue;

  public string Street { get; }
  public string Locality { get; }
  public string? PostalCode { get; }
  public string? Region { get; }
  public string Country { get; }

  public AddressUnit(string street, string locality, string country, string? postalCode = null, string? region = null, bool isVerified = false) : base(isVerified)
  {
    Street = street.Trim();
    Locality = locality.Trim();
    PostalCode = postalCode?.CleanTrim();
    Region = region?.CleanTrim();
    Country = country.Trim();
    new AddressValidator().ValidateAndThrow(this);
  }

  public static AddressUnit? TryCreate(string? street, string? locality, string? country, string? postalCode = null, string? region = null, bool isVerified = false)
  {
    if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(locality) || string.IsNullOrWhiteSpace(country))
    {
      return null;
    }

    return new(street, locality, country, postalCode, region, isVerified);
  }

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
