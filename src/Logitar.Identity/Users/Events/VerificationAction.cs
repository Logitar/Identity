namespace Logitar.Identity.Users.Events;

/// <summary>
/// Represents the verification actions that can be performed on a contact information.
/// </summary>
public enum VerificationAction
{
  /// <summary>
  /// Nothing will happen.
  /// </summary>
  None,

  /// <summary>
  /// The contact information will be verified.
  /// </summary>
  Verify,

  /// <summary>
  /// The contact information will be unverified.
  /// </summary>
  Unverify
}
