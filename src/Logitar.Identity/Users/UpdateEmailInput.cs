namespace Logitar.Identity.Users;

/// <summary>
/// The email address update input data.
/// </summary>
public record UpdateEmailInput : SaveEmailInput
{
  /// <summary>
  /// Gets or sets a value indicating whether or not the email address will be verified. If false,
  /// the email address will be unverified if its modified.
  /// </summary>
  public bool Verify { get; set; }
}
