using Logitar.Identity.Contracts.Settings;

namespace Logitar.Identity.Domain.Settings;

/// <summary>
/// Represents a resolver for role settings, allowing developers to customize how those settings are resolved.
/// </summary>
public interface IRoleSettingsResolver
{
  /// <summary>
  /// Resolves the role settings.
  /// </summary>
  /// <returns>The role settings.</returns>
  IRoleSettings Resolve();
}
