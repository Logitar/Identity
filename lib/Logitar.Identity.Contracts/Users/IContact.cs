namespace Logitar.Identity.Contracts.Users;

/// <summary>
/// Defines person contact informations.
/// </summary>
public interface IContact
{
  /// <summary>
  /// Gets a value indicating whether or not the contact is verified.
  /// </summary>
  bool IsVerified { get; }
}
