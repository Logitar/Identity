using Logitar.Identity.Domain.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Infrastructure.Tokens;

/// <summary>
/// Implements methods to manage JSON Web tokens.
/// </summary>
public class JsonWebTokenManager : ITokenManager
{
  /// <summary>
  /// Gets the JSON Web token handler.
  /// </summary>
  protected virtual JwtSecurityTokenHandler TokenHandler { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonWebTokenManager"/> class.
  /// </summary>
  public JsonWebTokenManager() : this(new())
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonWebTokenManager"/> class.
  /// </summary>
  /// <param name="tokenHandler">The JSON Web token handler.</param>
  public JsonWebTokenManager(JwtSecurityTokenHandler tokenHandler)
  {
    TokenHandler = tokenHandler;
    TokenHandler.InboundClaimTypeMap.Clear();
  }

  /// <summary>
  /// Creates a token for the specified subject, using the specified signing secret and creation options.
  /// </summary>
  /// <param name="subject">The subject of the token.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="options">The creation options.</param>
  /// <returns>The created token.</returns>
  public virtual CreatedToken Create(ClaimsIdentity subject, string secret, CreateTokenOptions? options)
  {
    return Create(new CreateTokenParameters(subject, secret, options));
  }
  /// <summary>
  /// Creates a token with the specified parameters.
  /// </summary>
  /// <param name="parameters">The creation parameters.</param>
  /// <returns>The created token.</returns>
  public virtual CreatedToken Create(CreateTokenParameters parameters)
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

    return new CreatedToken(securityToken, tokenString);
  }

  /// <summary>
  /// Validates a token using the specified signing secret and validation options.
  /// </summary>
  /// <param name="token">The token to validate.</param>
  /// <param name="secret">The signing secret.</param>
  /// <param name="options">The validation options.</param>
  /// <returns>The validated token.</returns>
  public virtual ValidatedToken Validate(string token, string secret, ValidateTokenOptions? options)
  {
    return Validate(new ValidateTokenParameters(token, secret, options));
  }
  /// <summary>
  /// Validates a token with the specified parameters.
  /// </summary>
  /// <param name="parameters">The validation parameters.</param>
  /// <returns>The validated token.</returns>
  public virtual ValidatedToken Validate(ValidateTokenParameters parameters)
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

    // TODO(fpion): if consumable, validate token is not blacklisted

    ClaimsPrincipal claimsPrincipal = TokenHandler.ValidateToken(parameters.Token, validationParameters, out SecurityToken securityToken);

    // TODO(fpion): if consumable, blacklist token

    return new ValidatedToken(claimsPrincipal, securityToken);
  }

  /// <summary>
  /// Ensures the specified date time is using the UTC time zone.
  /// </summary>
  /// <param name="value">The date time.</param>
  /// <returns>The universal date time, or null if value was null.</returns>
  protected virtual DateTime? AsUniversalTime(DateTime? value) => value.HasValue ? AsUniversalTime(value.Value) : null;
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
  };

  /// <summary>
  /// Creates a symmetric security key from the specified secret string.
  /// </summary>
  /// <param name="secret">The secret string.</param>
  /// <returns>The symmetric security key.</returns>
  protected virtual SymmetricSecurityKey GetSecurityKey(string secret) => new(Encoding.ASCII.GetBytes(secret));
}

// TODO(fpion): unit tests
