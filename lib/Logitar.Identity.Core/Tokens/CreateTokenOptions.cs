using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Core.Tokens;

/// <summary>
/// Represents token creation options.
/// </summary>
public record CreateTokenOptions
{
  /// <summary>
  /// Gets or sets the token type. This defaults to 'JWT'.
  /// </summary>
  public string Type { get; set; } = "JWT";
  /// <summary>
  /// Gets or sets the signing algorithm. This defaults to 'HS256'.
  /// </summary>
  public string SigningAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;

  /// <summary>
  /// Gets or sets the token audience.
  /// </summary>
  public string? Audience { get; set; }
  /// <summary>
  /// Gets or sets the token issuer.
  /// </summary>
  public string? Issuer { get; set; }

  /// <summary>
  /// Gets or sets the token expiration date and time. Unspecified date time kinds will be treated as UTC.
  /// </summary>
  public DateTime? Expires { get; set; }
  /// <summary>
  /// Gets or sets the date and time when the token was issued. Unspecified date time kinds will be treated as UTC.
  /// </summary>
  public DateTime? IssuedAt { get; set; }
  /// <summary>
  /// Gets or sets the date and time from when the token is valid. Unspecified date time kinds will be treated as UTC.
  /// </summary>
  public DateTime? NotBefore { get; set; }
}
