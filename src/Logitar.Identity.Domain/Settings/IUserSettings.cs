namespace Logitar.Identity.Domain.Settings;

/// <summary>
/// The settings applying to users.
/// </summary>
public interface IUserSettings
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
