namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate unique names.
/// </summary>
public interface IUniqueNameSettings
{
  /// <summary>
  /// Gets the list of allowed characters.
  /// </summary>
  string? AllowedCharacters { get; }
}
