namespace Logitar.Identity.Contracts.Users;

/// <summary>
/// Defines email addresses.
/// </summary>
public interface IEmail : IContact
{
  /// <summary>
  /// Gets the email address.
  /// </summary>
  string Address { get; }
}
