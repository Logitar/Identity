namespace Logitar.Identity.Contracts.Users;

/// <summary>
/// Defines person contact informations.
/// </summary>
public interface IContact
{
  /// <summary>
  /// Gets a value indicating whether the contact is verified or not.
  /// </summary>
  bool IsVerified { get; }
}
