using Logitar.EventSourcing;
using System.Text;

namespace Logitar.Identity.Users;

/// <summary>
/// Represents the postal address of an user.
/// </summary>
public record ReadOnlyAddress : ReadOnlyContact
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyAddress"/> record using the specified arguments.
  /// </summary>
  /// <param name="line1">The primary line of the postal address.</param>
  /// <param name="locality">The locality of the postal address.</param>
  /// <param name="country">The country of the postal address.</param>
  /// <param name="line2">The secondary line of the postal address.</param>
  /// <param name="postalCode">The postal code of the postal address.</param>
  /// <param name="region">The region of the postal address.</param>
  /// <param name="isVerified">A value indicating whether or not the postal address is verified.</param>
  public ReadOnlyAddress(string line1, string locality, string country, string? line2 = null,
    string? postalCode = null, string? region = null, bool isVerified = false) : base(isVerified)
  {
    Line1 = line1.Trim();
    Line2 = line2?.CleanTrim();
    Locality = locality.Trim();
    PostalCode = postalCode?.CleanTrim();
    Country = country.Trim();
    Region = region?.CleanTrim();
  }

  /// <summary>
  /// Gets the primary line of the postal address.
  /// </summary>
  public string Line1 { get; } = string.Empty;

  /// <summary>
  /// Gets the secondary line of the postal address.
  /// </summary>
  public string? Line2 { get; }

  /// <summary>
  /// Gets the locality of the postal address.
  /// </summary>
  public string Locality { get; } = string.Empty;

  /// <summary>
  /// Gets the postal code of the postal address.
  /// </summary>
  public string? PostalCode { get; }

  /// <summary>
  /// Gets the country of the postal address.
  /// </summary>
  public string Country { get; } = string.Empty;

  /// <summary>
  /// Gets the region of the postal address.
  /// </summary>
  public string? Region { get; }

  /// <summary>
  /// Returns a formatted string representation of the postal address.
  /// </summary>
  /// <returns>The string representation.</returns>
  public string ToFormattedString()
  {
    StringBuilder sb = new();

    sb.AppendLine(Line1);

    if (Line2 != null)
    {
      sb.AppendLine(Line2);
    }

    sb.AppendLine(Locality);
    if (Region != null)
    {
      sb.Append(' ').Append(Region);
    }
    if (PostalCode != null)
    {
      sb.Append(' ').Append(PostalCode);
    }
    sb.AppendLine();

    sb.AppendLine(Country);

    return sb.ToString();
  }
}
