namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate unique names.
/// </summary>
public interface IUniqueNameSettings // TODO(fpion): move to Contracts
{
  /// <summary>
  /// Gets the list of allowed characters.
  /// </summary>
  string? AllowedCharacters { get; }
}
