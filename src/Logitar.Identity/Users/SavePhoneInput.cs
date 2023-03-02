namespace Logitar.Identity.Users;

/// <summary>
/// The base email address input data.
/// </summary>
public abstract record SavePhoneInput
{
  /// <summary>
  /// Gets the country code of the phone.
  /// </summary>
  public string? CountryCode { get; set; }

  /// <summary>
  /// Gets the phone number.
  /// </summary>
  public string Number { get; set; } = string.Empty;

  /// <summary>
  /// Gets the extension of the phone.
  /// </summary>
  public string? Extension { get; set; }
}
