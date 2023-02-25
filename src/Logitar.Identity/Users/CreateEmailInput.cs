namespace Logitar.Identity.Users;

/// <summary>
/// The email address creation input data.
/// </summary>
public record CreateEmailInput : SaveEmailInput
{
  /// <summary>
  /// Gets or sets a value indicating whether or not the email address is verified.
  /// </summary>
  public bool IsVerified { get; set; }
}
