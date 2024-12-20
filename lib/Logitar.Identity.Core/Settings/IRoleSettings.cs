namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings applying to roles.
/// </summary>
public interface IRoleSettings // TODO(fpion): move to Contracts
{
  /// <summary>
  /// Gets the role unique name validation settings.
  /// </summary>
  IUniqueNameSettings UniqueName { get; }
}
