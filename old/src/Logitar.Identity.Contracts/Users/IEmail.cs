namespace Logitar.Identity.Contracts.Users;

/// <summary>
/// Describes email addresses.
/// </summary>
public interface IEmail
{
  /// <summary>
  /// Gets the email address.
  /// </summary>
  string Address { get; }
}
