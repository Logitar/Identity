namespace Logitar.Identity.Domain.Settings;

/// <summary>
/// The settings applying to users.
/// </summary>
public record UserSettings : IUserSettings
{
  /// <summary>
  /// Gets or sets the user unique name validation settings.
  /// </summary>
  public IUniqueNameSettings UniqueName { get; set; } = new UniqueNameSettings();
  /// <summary>
  /// Gets or sets the password validation settings.
  /// </summary>
  public IPasswordSettings Password { get; set; } = new PasswordSettings();

  /// <summary>
  /// Gets a value indicating whether or not email addresses are unique.
  /// </summary>
  public bool RequireUniqueEmail { get; set; } = false;
}
