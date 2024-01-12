namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents a contact information.
/// </summary>
public interface IContact
{
  /// <summary>
  /// Gets a value indicating whether or not the contact is verified.
  /// </summary>
  bool IsVerified { get; }
}
