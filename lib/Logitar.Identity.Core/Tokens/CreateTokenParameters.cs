namespace Logitar.Identity.Core.Tokens;

/// <summary>
/// Represents token creation parameters.
/// </summary>
public record CreateTokenParameters : CreateTokenOptions
{
  /// <summary>
  /// Gets or sets the token subject.
  /// </summary>
  public ClaimsIdentity Subject { get; set; }
  /// <summary>
  /// Gets or sets the signing secret.
  /// </summary>
  public string Secret { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="CreateTokenParameters"/> class.
  /// </summary>
  public CreateTokenParameters() : this(new(), string.Empty)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="CreateTokenParameters"/> class.
  /// </summary>
  /// <param name="subject">The token subject.</param>
  /// <param name="secret">The signing secret.</param>
  public CreateTokenParameters(ClaimsIdentity subject, string secret)
  {
    Subject = subject;
    Secret = secret;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="CreateTokenParameters"/> class.
  /// </summary>
  /// <param name="subject">The token subject.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="options">The token creation options.</param>
  public CreateTokenParameters(ClaimsIdentity subject, string secret, CreateTokenOptions? options) : this(subject, secret)
  {
    if (options != null)
    {
      Type = options.Type;
      SigningAlgorithm = options.SigningAlgorithm;
      Audience = options.Audience;
      Issuer = options.Issuer;
      Expires = options.Expires;
      IssuedAt = options.IssuedAt;
      NotBefore = options.NotBefore;
    }
  }
}
