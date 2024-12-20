using Logitar.Identity.Contracts.Users;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents a contact information of a person.
/// </summary>
public abstract record Contact : IContact
{
  /// <summary>
  /// Gets a value indicating whether or not the contact is verified.
  /// </summary>
  public bool IsVerified { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Contact"/> class.
  /// </summary>
  /// <param name="isVerified">A value indicating whether or not the contact is verified.</param>
  protected Contact(bool isVerified)
  {
    IsVerified = isVerified;
  }
}
