namespace Logitar.Identity.Users;

/// <summary>
/// The password change input data.
/// </summary>
public record ChangePasswordInput
{
  /// <summary>
  /// Gets or sets the actual password of the user.
  /// </summary>
  public string Current { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the new password of the user.
  /// </summary>
  public string Password { get; set; } = string.Empty;
}
