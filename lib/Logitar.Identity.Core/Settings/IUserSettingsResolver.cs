using Logitar.Identity.Contracts.Settings;

namespace Logitar.Identity.Core.Settings;

/// <summary>
/// Represents a resolver for user settings, allowing developers to customize how those settings are resolved.
/// </summary>
public interface IUserSettingsResolver
{
  /// <summary>
  /// Resolves the user settings.
  /// </summary>
  /// <returns>The user settings.</returns>
  IUserSettings Resolve();
}
