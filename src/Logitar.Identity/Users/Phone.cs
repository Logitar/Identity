namespace Logitar.Identity.Users;

/// <summary>
/// The output representation of a phone number.
/// </summary>
public record Phone : Contact
{
  /// <summary>
  /// Gets or sets the country code of the phone number.
  /// </summary>
  public string? CountryCode { get; set; }

  /// <summary>
  /// Gets or sets the phone number.
  /// </summary>
  public string Number { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the extension of the phone number.
  /// </summary>
  public string? Extension { get; set; }

  /// <summary>
  /// Gets or sets the E.164 formatted user's phone number.
  /// </summary>
  public string? E164Formatted { get; set; }
}
