namespace Logitar.Identity.Users;

/// <summary>
/// The output representation of a postal address.
/// </summary>
public record Address : Contact
{
  /// <summary>
  /// Gets or sets the primary line of the postal address.
  /// </summary>
  public string Line1 { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the secondary line of the postal address.
  /// </summary>
  public string? Line2 { get; set; }

  /// <summary>
  /// Gets or sets the locality of the postal address.
  /// </summary>
  public string Locality { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the postal code of the postal address.
  /// </summary>
  public string? PostalCode { get; set; }

  /// <summary>
  /// Gets or sets the country of the postal address.
  /// </summary>
  public string Country { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the region of the postal address.
  /// </summary>
  public string? Region { get; set; }

  /// <summary>
  /// Gets or sets the formatted postal address.
  /// </summary>
  public string Formatted { get; set; } = string.Empty;
}
