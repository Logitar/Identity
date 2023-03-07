namespace Logitar.Identity.Tokens;

/// <summary>
/// The token creation input data.
/// </summary>
public record CreateTokenInput
{
  /// <summary>
  /// Gets or sets the lifetime of the token, in seconds.
  /// </summary>
  public int? Lifetime { get; set; }
  /// <summary>
  /// Gets or sets the purpose of the token.
  /// </summary>
  public string? Purpose { get; set; }
  /// <summary>
  /// Gets or sets the identifier or unique name of the realm in which the token should be created.
  /// </summary>
  public string? Realm { get; set; }

  /// <summary>
  /// Gets or sets the value of the subject claim.
  /// </summary>
  public string? Subject { get; set; }

  /// <summary>
  /// Gets or sets the list of claims to embed into the token.
  /// </summary>
  public IEnumerable<Claim>? Claims { get; set; }
}
