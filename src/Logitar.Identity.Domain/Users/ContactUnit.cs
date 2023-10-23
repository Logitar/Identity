namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents a contact information of a person.
/// </summary>
public record ContactUnit
{
  /// <summary>
  /// Gets a value indicating whether or not the contact is verified.
  /// </summary>
  public bool IsVerified { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ContactUnit"/> class.
  /// </summary>
  /// <param name="isVerified">A value indicating whether or not the contact is verified.</param>
  protected ContactUnit(bool isVerified = false)
  {
    IsVerified = isVerified;
  }
}
