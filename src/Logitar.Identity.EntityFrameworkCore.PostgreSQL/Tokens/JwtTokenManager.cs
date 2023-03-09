using Logitar.Identity.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Tokens;

/// <summary>
/// Exposes methods to manage JSON Web Tokens in the identity system.
/// </summary>
internal class JwtTokenManager : ITokenManager
{
  /// <summary>
  /// The default algorithm used to sign JSON Web Tokens.
  /// </summary>
  private const string DefaultAlgorithm = SecurityAlgorithms.HmacSha256;

  /// <summary>
  /// Initializes the static <see cref="JwtTokenManager"/> class members.
  /// </summary>
  static JwtTokenManager()
  {
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
  }

  /// <summary>
  /// The JSON Web Token handler.
  /// </summary>
  private readonly JwtSecurityTokenHandler _tokenHandler = new();

  /// <summary>
  /// Creates a JSON Web Token using the specified arguments.
  /// </summary>
  /// <param name="subject">The subject identity represented by the JSON Web token.</param>
  /// <param name="secret">The secret used to sign the token.</param>
  /// <param name="algorithm">The algorithm used to sign the token.</param>
  /// <param name="expires">The date and time when the token expires.</param>
  /// <param name="audience">The audience of the token.</param>
  /// <param name="issuer">The issuer of the token.</param>
  /// <returns>The token string.</returns>
  public string Create(ClaimsIdentity subject, string? secret, string? algorithm, DateTime? expires, string? audience, string? issuer)
  {
    SigningCredentials? signingCredentials = secret == null ? null : new(GetSecurityKey(secret), algorithm ?? DefaultAlgorithm);

    SecurityTokenDescriptor tokenDescriptor = new()
    {
      Audience = audience,
      Expires = expires,
      Issuer = issuer,
      SigningCredentials = signingCredentials,
      Subject = subject
    };

    SecurityToken token = _tokenHandler.CreateToken(tokenDescriptor);

    return _tokenHandler.WriteToken(token);
  }

  /// <summary>
  /// Resolves a security key using the specified secret.
  /// </summary>
  /// <param name="secret">The secret of the key.</param>
  /// <returns>The security key.</returns>
  private static SecurityKey GetSecurityKey(string secret) => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
}
