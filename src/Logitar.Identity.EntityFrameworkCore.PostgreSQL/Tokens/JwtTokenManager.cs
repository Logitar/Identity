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
  /// The JSON Web Token blacklist.
  /// </summary>
  private readonly IJwtBlacklist _blacklist;
  /// <summary>
  /// The JSON Web Token handler.
  /// </summary>
  private readonly JwtSecurityTokenHandler _tokenHandler = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="JwtTokenManager"/> class using the specified arguments.
  /// </summary>
  /// <param name="blacklist">The JSON Web Token blacklist.</param>
  public JwtTokenManager(IJwtBlacklist blacklist)
  {
    _blacklist = blacklist;
  }

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
  /// Validates the specified security token using the specified arguments.
  /// </summary>
  /// <param name="token">The security token to validate.</param>
  /// <param name="secret">The secret used to sign the token.</param>
  /// <param name="audience">The audience of the token.</param>
  /// <param name="issuer">The issuer of the token.</param>
  /// <param name="purpose">The purpose of the token.</param>
  /// <param name="consume">If true, the token will be consumed. A consumed cannot be used again.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The claims principal.</returns>
  public async Task<ClaimsPrincipal> ValidateAsync(string token, string? secret, string? audience, string? issuer, string? purpose, bool consume, CancellationToken cancellationToken)
  {
    SecurityKey? key = secret == null ? null : GetSecurityKey(secret);

    TokenValidationParameters validationParameters = new()
    {
      IssuerSigningKey = key,
      ValidAudience = audience,
      ValidIssuer = issuer,
      ValidateAudience = audience != null,
      ValidateIssuer = issuer != null,
      ValidateIssuerSigningKey = key != null
    };

    ClaimsPrincipal principal = _tokenHandler.ValidateToken(token, validationParameters, out _);

    IEnumerable<Guid> ids = principal.FindAll(Rfc7519ClaimTypes.JwtId).Select(x => Guid.Parse(x.Value));
    if (ids.Any())
    {
      IEnumerable<Guid> blacklisted = await _blacklist.GetBlacklistedAsync(ids, cancellationToken);
      if (blacklisted.Any())
      {
        throw new SecurityTokenBlacklistedException(blacklisted);
      }
    }

    if (purpose != null)
    {
      IEnumerable<System.Security.Claims.Claim> claims = principal.FindAll(CustomClaimTypes.Purpose);
      HashSet<string> purposes = claims.SelectMany(x => x.Value.Split().Select(y => y.ToLower())).ToHashSet();
      if (!purposes.Contains(purpose.ToLower()))
      {
        throw new InvalidSecurityTokenPurposeException(purpose, purposes);
      }
    }

    if (consume)
    {
      DateTime? expiresOn = principal.FindFirst(Rfc7519ClaimTypes.Expires)
        ?.GetDateTime()
        .Add(validationParameters.ClockSkew);

      await _blacklist.BlacklistAsync(ids, expiresOn, cancellationToken);
    }

    return principal;
  }

  /// <summary>
  /// Resolves a security key using the specified secret.
  /// </summary>
  /// <param name="secret">The secret of the key.</param>
  /// <returns>The security key.</returns>
  private static SecurityKey GetSecurityKey(string secret) => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
}
