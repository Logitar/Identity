namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings applying to roles.
/// </summary>
public interface IRoleSettings
{
  /// <summary>
  /// Gets the role unique name validation settings.
  /// </summary>
  IUniqueNameSettings UniqueName { get; }
}
