using Logitar.Identity.Actors;

namespace Logitar.Identity.Users;

/// <summary>
/// The output representation of a contact information.
/// </summary>
public abstract record Contact
{
  /// <summary>
  /// Gets or sets the actor who verified the contact.
  /// </summary>
  public Actor? VerifiedBy { get; set; }
  /// <summary>
  /// Gets or sets the date and time when the contact was verified.
  /// </summary>
  public DateTime? VerifiedOn { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the contact is verified.
  /// </summary>
  public bool IsVerified { get; set; }
}
