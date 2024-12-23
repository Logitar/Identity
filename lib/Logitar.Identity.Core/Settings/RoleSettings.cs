using Logitar.Identity.Contracts.Settings;

namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings applying to roles.
/// </summary>
public record RoleSettings : IRoleSettings
{
  /// <summary>
  /// Gets or sets the role unique name validation settings.
  /// </summary>
  public IUniqueNameSettings UniqueName { get; set; } = new UniqueNameSettings();
}
