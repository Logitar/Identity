namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings applying to users.
/// </summary>
public interface IUserSettings // TODO(fpion): move to Contracts
{
  /// <summary>
  /// Gets the user unique name validation settings.
  /// </summary>
  IUniqueNameSettings UniqueName { get; }
  /// <summary>
  /// Gets the password validation settings.
  /// </summary>
  IPasswordSettings Password { get; }

  /// <summary>
  /// Gets a value indicating whether or not email addresses are unique.
  /// </summary>
  bool RequireUniqueEmail { get; }
}
