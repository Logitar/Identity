namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents a contact information of a person.
/// </summary>
public abstract record Contact
{
  /// <summary>
  /// Gets a value indicating whether the contact is verified or not.
  /// </summary>
  public bool IsVerified { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Contact"/> class.
  /// </summary>
  /// <param name="isVerified">A value indicating whether the contact is verified or not.</param>
  protected Contact(bool isVerified)
  {
    IsVerified = isVerified;
  }
}
