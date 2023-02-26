namespace Logitar.Identity.Sessions;

/// <summary>
/// The user session refresh input data.
/// </summary>
public record RefreshSessionInput : SaveSessionInput
{
  /// <summary>
  /// Gets or sets the refresh token of the user session.
  /// </summary>
  public string RefreshToken { get; set; } = string.Empty;
}
