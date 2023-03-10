namespace Logitar.Identity.Tokens;

/// <summary>
/// The token creation input data.
/// </summary>
public record CreateTokenInput
{
  /// <summary>
  /// Gets a value indicating whether or not the token can be consumed. Once consume, a token cannot
  /// be used again.
  /// </summary>
  public bool IsConsumable { get; set; }
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
  /// If provided, the realm will be retrieved and the Secret property will be ignored.
  /// </summary>
  public string? Realm { get; set; }
  /// <summary>
  /// Gets or sets the secret used to sign the JSON Web Token.
  /// </summary>
  public string? Secret { get; set; }
  /// <summary>
  /// Gets or sets the algorithm used to sign the JSON Web Token.
  /// </summary>
  public string? Algorithm { get; set; }

  /// <summary>
  /// Gets or sets the audience of the JSON Web Token.
  /// </summary>
  public string? Audience { get; set; }
  /// <summary>
  /// Gets or sets the issuer of the JSON Web Token.
  /// </summary>
  public string? Issuer { get; set; }

  /// <summary>
  /// Gets or sets the value of the subject claim.
  /// </summary>
  public string? Subject { get; set; }

  /// <summary>
  /// Gets or sets the list of claims to embed into the token.
  /// </summary>
  public IEnumerable<Claim>? Claims { get; set; }
}
