namespace Logitar.Identity.Domain.Tokens;

/// <summary>
/// Represents token validation parameters.
/// </summary>
public record ValidateTokenParameters : ValidateTokenOptions
{
  /// <summary>
  /// Gets or sets the token to validate.
  /// </summary>
  public string Token { get; set; }
  /// <summary>
  /// Gets or sets the signing secret.
  /// </summary>
  public string Secret { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidateTokenParameters"/> class.
  /// </summary>
  public ValidateTokenParameters() : this(string.Empty, string.Empty)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidateTokenParameters"/> class.
  /// </summary>
  /// <param name="token">The token to validate.</param>
  /// <param name="secret">The signing secret.</param>
  public ValidateTokenParameters(string token, string secret)
  {
    Token = token;
    Secret = secret;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidateTokenParameters"/> class.
  /// </summary>
  /// <param name="token">The token to validate.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="options">The token validation options.</param>
  public ValidateTokenParameters(string token, string secret, ValidateTokenOptions? options) : this(token, secret)
  {
    if (options != null)
    {
      ValidTypes.AddRange(options.ValidTypes);
      ValidAudiences.AddRange(options.ValidAudiences);
      ValidIssuers.AddRange(options.ValidIssuers);
    }
  }
}

// TODO(fpion): unit tests
