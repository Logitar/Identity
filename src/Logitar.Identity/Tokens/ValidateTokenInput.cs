namespace Logitar.Identity.Tokens;

/// <summary>
/// The token validation input data.
/// </summary>
public record ValidateTokenInput
{
  /// <summary>
  /// Gets or sets the token to validate.
  /// </summary>
  public string Token { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the expected purpose of the token.
  /// </summary>
  public string? Purpose { get; set; }
  /// <summary>
  /// Gets or sets the identifier or unique name of the realm in which the token should be validated.
  /// If provided, the realm will be retrieved and the Secret property will be ignored.
  /// </summary>
  public string? Realm { get; set; }
  /// <summary>
  /// Gets or sets the secret used to validate the token.
  /// </summary>
  public string? Secret { get; set; }

  /// <summary>
  /// Gets or sets the expected audience of the token.
  /// </summary>
  public string? Audience { get; set; }
  /// <summary>
  /// Gets or sets the expected issuer of the token.
  /// </summary>
  public string? Issuer { get; set; }
}
