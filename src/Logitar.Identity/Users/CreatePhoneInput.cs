namespace Logitar.Identity.Users;

/// <summary>
/// The phone number creation input data.
/// </summary>
public record CreatePhoneInput : SavePhoneInput
{
  /// <summary>
  /// Gets or sets a value indicating whether or not the phone number is verified.
  /// </summary>
  public bool IsVerified { get; set; }
}
