namespace Logitar.Identity.Sessions;

/// <summary>
/// The account sign-in input data.
/// </summary>
public record SignInInput
{
  /// <summary>
  /// Gets or sets the identifier or unique name of the realm in which the user resides.
  /// </summary>
  public string Realm { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the unique name of the user (case-insensitive). If the realm requires unique
  /// email addresses, the email address can be supplied in this field.
  /// </summary>
  public string Username { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the password of the user.
  /// </summary>
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets a value indicating whether or not the resulting user session will be persistent.
  /// </summary>
  public bool Remember { get; set; }

  /// <summary>
  /// Gets or sets the custom attributes of the user session.
  /// </summary>
  public IEnumerable<CustomAttribute>? CustomAttributes { get; set; }
}
