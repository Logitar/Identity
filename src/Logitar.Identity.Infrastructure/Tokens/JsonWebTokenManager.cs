using Logitar.Identity.Domain.Tokens;
using Logitar.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Infrastructure.Tokens;

/// <summary>
/// Implements methods to manage JSON Web tokens.
/// </summary>
public class JsonWebTokenManager : ITokenManager
{
  /// <summary>
  /// Gets the token blacklist.
  /// </summary>
  protected virtual ITokenBlacklist TokenBlacklist { get; }
  /// <summary>
  /// Gets the JSON Web token handler.
  /// </summary>
  protected virtual JwtSecurityTokenHandler TokenHandler { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonWebTokenManager"/> class.
  /// </summary>
  /// <param name="tokenBlacklist">The token blacklist.</param>
  public JsonWebTokenManager(ITokenBlacklist tokenBlacklist) : this(tokenBlacklist, new())
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonWebTokenManager"/> class.
  /// </summary>
  /// <param name="tokenBlacklist">The token blacklist.</param>
  /// <param name="tokenHandler">The JSON Web token handler.</param>
  public JsonWebTokenManager(ITokenBlacklist tokenBlacklist, JwtSecurityTokenHandler tokenHandler)
  {
    TokenBlacklist = tokenBlacklist;
    TokenHandler = tokenHandler;
    TokenHandler.InboundClaimTypeMap.Clear();
  }

  /// <summary>
  /// Creates a token for the specified subject, using the specified signing secret.
  /// </summary>
  /// <param name="subject">The subject of the token.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The created token.</returns>
  public virtual async Task<CreatedToken> CreateAsync(ClaimsIdentity subject, string secret, CancellationToken cancellationToken)
  {
    return await CreateAsync(subject, secret, options: null, cancellationToken);
  }
  /// <summary>
  /// Creates a token for the specified subject, using the specified signing secret and creation options.
  /// </summary>
  /// <param name="subject">The subject of the token.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="options">The creation options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The created token.</returns>
  public virtual async Task<CreatedToken> CreateAsync(ClaimsIdentity subject, string secret, CreateTokenOptions? options, CancellationToken cancellationToken)
  {
    return await CreateAsync(new CreateTokenParameters(subject, secret, options), cancellationToken);
  }
  /// <summary>
  /// Creates a token with the specified parameters.
  /// </summary>
  /// <param name="parameters">The creation parameters.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The created token.</returns>
  public virtual Task<CreatedToken> CreateAsync(CreateTokenParameters parameters, CancellationToken cancellationToken)
  {
    SigningCredentials signingCredentials = new(GetSecurityKey(parameters.Secret), parameters.SigningAlgorithm);

    SecurityTokenDescriptor tokenDescriptor = new()
    {
      Audience = parameters.Audience,
      Expires = AsUniversalTime(parameters.Expires),
      IssuedAt = AsUniversalTime(parameters.IssuedAt),
      Issuer = parameters.Issuer,
      NotBefore = AsUniversalTime(parameters.NotBefore),
      SigningCredentials = signingCredentials,
      Subject = parameters.Subject,
      TokenType = parameters.Type
    };

    SecurityToken securityToken = TokenHandler.CreateToken(tokenDescriptor);
    string tokenString = TokenHandler.WriteToken(securityToken);

    CreatedToken createdToken = new(securityToken, tokenString);
    return Task.FromResult(createdToken);
  }

  /// <summary>
  /// Validates a token using the specified signing secret.
  /// </summary>
  /// <param name="token">The token to validate.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The validated token.</returns>
  public virtual async Task<ValidatedToken> ValidateAsync(string token, string secret, CancellationToken cancellationToken)
  {
    return await ValidateAsync(token, secret, options: null, cancellationToken);
  }
  /// <summary>
  /// Validates a token using the specified signing secret and validation options.
  /// </summary>
  /// <param name="token">The token to validate.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="options">The validation options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The validated token.</returns>
  public virtual async Task<ValidatedToken> ValidateAsync(string token, string secret, ValidateTokenOptions? options, CancellationToken cancellationToken)
  {
    return await ValidateAsync(new ValidateTokenParameters(token, secret, options), cancellationToken);
  }
  /// <summary>
  /// Validates a token with the specified parameters.
  /// </summary>
  /// <param name="parameters">The validation parameters.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The validated token.</returns>
  public virtual async Task<ValidatedToken> ValidateAsync(ValidateTokenParameters parameters, CancellationToken cancellationToken)
  {
    TokenValidationParameters validationParameters = new()
    {
      IssuerSigningKey = GetSecurityKey(parameters.Secret),
      ValidAudiences = parameters.ValidAudiences,
      ValidIssuers = parameters.ValidIssuers,
      ValidateAudience = parameters.ValidAudiences.Count > 0,
      ValidateIssuer = parameters.ValidIssuers.Count > 0,
      ValidateIssuerSigningKey = true
    };
    if (parameters.ValidTypes.Count > 0)
    {
      validationParameters.ValidTypes = parameters.ValidTypes;
    }

    ClaimsPrincipal claimsPrincipal = TokenHandler.ValidateToken(parameters.Token, validationParameters, out SecurityToken securityToken);

    HashSet<string> tokenIds = claimsPrincipal.FindAll(Rfc7519ClaimNames.TokenId).Select(claim => claim.Value).ToHashSet();
    if (tokenIds.Count > 0)
    {
      IEnumerable<string> blacklistedIds = await TokenBlacklist.GetBlacklistedAsync(tokenIds, cancellationToken);
      if (blacklistedIds.Any())
      {
        throw new SecurityTokenBlacklistedException(blacklistedIds);
      }
    }

    if (parameters.Consume)
    {
      Claim? expiresClaim = claimsPrincipal.FindAll(Rfc7519ClaimNames.ExpirationTime).OrderBy(x => x.Value).FirstOrDefault();
      DateTime? expiresOn = expiresClaim == null ? null : ClaimHelper.ExtractDateTime(expiresClaim).Add(validationParameters.ClockSkew);

      await TokenBlacklist.BlacklistAsync(tokenIds, expiresOn, cancellationToken);
    }

    return new ValidatedToken(claimsPrincipal, securityToken);
  }

  /// <summary>
  /// Ensures the specified date time is using the UTC time zone.
  /// </summary>
  /// <param name="value">The date time.</param>
  /// <returns>The universal date time, or null if value was null.</returns>
  protected virtual DateTime? AsUniversalTime(DateTime? value) => value.HasValue ? AsUniversalTime(value.Value) : null; // TODO(fpion): refactor
  /// <summary>
  /// Ensures the specified date time is using the UTC time zone.
  /// </summary>
  /// <param name="value">The date time.</param>
  /// <returns>The universal date time.</returns>
  protected virtual DateTime AsUniversalTime(DateTime value) => value.Kind switch
  {
    DateTimeKind.Local => value.ToUniversalTime(),
    DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
    _ => value,
  }; // TODO(fpion): refactor

  /// <summary>
  /// Creates a symmetric security key from the specified secret string.
  /// </summary>
  /// <param name="secret">The secret string.</param>
  /// <returns>The symmetric security key.</returns>
  protected virtual SymmetricSecurityKey GetSecurityKey(string secret) => new(Encoding.ASCII.GetBytes(secret));
}

// TODO(fpion): unit tests
