namespace Logitar.Identity.Users;

/// <summary>
/// The phone number update input data.
/// </summary>
public record UpdatePhoneInput : SavePhoneInput
{
  /// <summary>
  /// Gets or sets a value indicating whether or not the phone number will be verified. If false,
  /// the phone number will be unverified if its modified.
  /// </summary>
  public bool Verify { get; set; }
}
