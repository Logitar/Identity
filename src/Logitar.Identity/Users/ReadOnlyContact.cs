namespace Logitar.Identity.Users;

/// <summary>
/// Represents the base of contact informations.
/// </summary>
public abstract record ReadOnlyContact
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyContact"/> record using the specified arguments.
  /// </summary>
  /// <param name="isVerified">A value indicating whether or not the contact information is verified.</param>
  protected ReadOnlyContact(bool isVerified)
  {
    IsVerified = isVerified;
  }

  /// <summary>
  /// Gets a value indicating whether or not the contact information is verified.
  /// </summary>
  public bool IsVerified { get; }
}
