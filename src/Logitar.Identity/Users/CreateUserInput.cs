namespace Logitar.Identity.Users;

/// <summary>
/// The user creation input data.
/// </summary>
public record CreateUserInput : SaveUserInput
{
  /// <summary>
  /// Gets or sets the identifier of the realm in which this user belongs.
  /// </summary>
  public Guid RealmId { get; set; }

  /// <summary>
  /// Gets or sets the unique name of the user (not case-sensitive).
  /// </summary>
  public string Username { get; set; } = string.Empty;
}
