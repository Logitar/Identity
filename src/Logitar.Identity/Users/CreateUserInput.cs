namespace Logitar.Identity.Users;

/// <summary>
/// The user creation input data.
/// </summary>
public record CreateUserInput : SaveUserInput
{
  /// <summary>
  /// Gets or sets the identifier or unique name of the realm in which the user belongs.
  /// </summary>
  public string Realm { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the unique name of the user (case-insensitive).
  /// </summary>
  public string Username { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the postal address of the user.
  /// </summary>
  public CreateAddressInput? Address { get; set; }

  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public CreateEmailInput? Email { get; set; }

  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public CreatePhoneInput? Phone { get; set; }
}
