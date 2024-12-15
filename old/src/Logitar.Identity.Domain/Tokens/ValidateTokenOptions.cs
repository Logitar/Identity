namespace Logitar.Identity.Domain.Tokens;

/// <summary>
/// Represents token validation options.
/// </summary>
public record ValidateTokenOptions
{
  /// <summary>
  /// Gets or sets the list of valid token types.
  /// </summary>
  public List<string> ValidTypes { get; set; } = [];

  /// <summary>
  /// Gets or sets the list of valid audiences.
  /// </summary>
  public List<string> ValidAudiences { get; set; } = [];
  /// <summary>
  /// Gets or sets the list of valid audiences.
  /// </summary>
  public List<string> ValidIssuers { get; set; } = [];

  /// <summary>
  /// Gets or sets a value indicating whether or not to blacklist the token identifiers if the token is valid. This should be set to true for one-time use tokens.
  /// </summary>
  public bool Consume { get; set; }
}
