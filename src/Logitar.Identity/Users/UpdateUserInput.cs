namespace Logitar.Identity.Users;

/// <summary>
/// The user update input data.
/// </summary>
public record UpdateUserInput : SaveUserInput
{
  /// <summary>
  /// Gets or sets the postal address of the user.
  /// </summary>
  public UpdateAddressInput? Address { get; set; }

  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public UpdateEmailInput? Email { get; set; }

  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public UpdatePhoneInput? Phone { get; set; }
}
