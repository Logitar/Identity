using Bogus;
using Logitar.Identity.Core.Tokens;
using Logitar.Security.Claims;
using Logitar.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Logitar.Identity.Infrastructure.Tokens;

[Trait(Traits.Category, Categories.Unit)]
public class JsonWebTokenManagerTests
{
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();
  private readonly string _tokenId = $"TokenId:{Guid.NewGuid()}";
  private readonly ClaimsIdentity _subject = new();
  private readonly string _secret = RandomStringGenerator.GetString(256 / 8);

  private readonly Mock<ITokenBlacklist> _tokenBlacklist = new();
  private readonly JwtSecurityTokenHandler _tokenHandler = new();
  private readonly JsonWebTokenManager _tokenManager;

  public JsonWebTokenManagerTests()
  {
    DateTime now = DateTime.Now;
    _subject.AddClaim(new(Rfc7519ClaimNames.Subject, $"UserId:{Guid.NewGuid()}"));
    _subject.AddClaim(new(Rfc7519ClaimNames.Username, _faker.Person.UserName));
    _subject.AddClaim(new(Rfc7519ClaimNames.EmailAddress, _faker.Person.Email));
    _subject.AddClaim(new(Rfc7519ClaimNames.IsEmailVerified, bool.TrueString.ToLower()));
    _subject.AddClaim(new(Rfc7519ClaimNames.PhoneNumber, _faker.Person.Phone));
    _subject.AddClaim(new(Rfc7519ClaimNames.IsPhoneVerified, bool.FalseString.ToLower()));
    _subject.AddClaim(new(Rfc7519ClaimNames.FirstName, _faker.Person.FirstName));
    _subject.AddClaim(new(Rfc7519ClaimNames.LastName, _faker.Person.LastName));
    _subject.AddClaim(new(Rfc7519ClaimNames.FullName, _faker.Person.FullName));
    _subject.AddClaim(new(Rfc7519ClaimNames.Gender, _faker.Person.Gender.ToString().ToLower()));
    _subject.AddClaim(new(Rfc7519ClaimNames.Locale, _faker.Locale));
    _subject.AddClaim(new(Rfc7519ClaimNames.Picture, _faker.Person.Avatar));
    _subject.AddClaim(new(Rfc7519ClaimNames.Roles, "manage_users manage_sessions"));
    _subject.AddClaim(new(Rfc7519ClaimNames.SessionId, $"SessionId:{Guid.NewGuid()}"));
    _subject.AddClaim(new(Rfc7519ClaimNames.TokenId, _tokenId));
    _subject.AddClaim(ClaimHelper.Create(Rfc7519ClaimNames.Birthdate, _faker.Person.DateOfBirth));
    _subject.AddClaim(ClaimHelper.Create(Rfc7519ClaimNames.UpdatedAt, now));
    _subject.AddClaim(ClaimHelper.Create(Rfc7519ClaimNames.AuthenticationTime, now));

    _tokenManager = new(_tokenBlacklist.Object, _tokenHandler);
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct token from parameters.")]
  public async Task CreateAsync_it_should_create_the_correct_token_from_parameters()
  {
    DateTime now = DateTime.Now;
    CreateTokenParameters parameters = new(_subject, _secret)
    {
      Type = "ID+JWT",
      Audience = "test_audience",
      Issuer = "test_issuer",
      Expires = now.AddMinutes(15),
      IssuedAt = now.AddSeconds(-15).ToUniversalTime(),
      NotBefore = new(now.Ticks, DateTimeKind.Unspecified)
    };
    CreatedToken createdToken = await _tokenManager.CreateAsync(parameters, _cancellationToken);
    AssertIsValid(createdToken, parameters);
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct token with options.")]
  public async Task CreateAsync_it_should_create_the_correct_token_with_options()
  {
    DateTime now = DateTime.Now;
    CreateTokenOptions options = new()
    {
      Type = "ID+JWT",
      Audience = "test_audience",
      Issuer = "test_issuer",
      Expires = now.AddMinutes(15),
      IssuedAt = now.AddSeconds(-15).ToUniversalTime(),
      NotBefore = new(now.Ticks, DateTimeKind.Unspecified)
    };
    CreatedToken createdToken = await _tokenManager.CreateAsync(_subject, _secret, options, _cancellationToken);
    AssertIsValid(createdToken, options);
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct token without options.")]
  public async Task CreateAsync_it_should_create_the_correct_token_without_options()
  {
    CreatedToken createdToken;

    createdToken = await _tokenManager.CreateAsync(_subject, _secret, _cancellationToken);
    AssertIsValid(createdToken);

    createdToken = await _tokenManager.CreateAsync(_subject, _secret, options: null, _cancellationToken);
    AssertIsValid(createdToken);
  }

  [Fact(DisplayName = "ctor: it should construct the correct instance.")]
  public void ctor_it_should_construct_the_correct_instance()
  {
    Assert.Empty(_tokenHandler.InboundClaimTypeMap);
  }

  [Fact(DisplayName = "ValidateAsync: it should blacklist token identifiers when consuming.")]
  public async Task ValidateAsync_it_should_blacklist_token_identifiers_when_consuming()
  {
    string[] tokenIds = [_tokenId];
    _tokenBlacklist.Setup(x => x.GetBlacklistedAsync(tokenIds, _cancellationToken)).ReturnsAsync([]);

    SecurityTokenDescriptor tokenDescriptor = new()
    {
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret)), SecurityAlgorithms.HmacSha256),
      Subject = _subject,
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    ValidateTokenParameters parameters = new(token, _secret)
    {
      Consume = true
    };
    ValidatedToken validatedToken = await _tokenManager.ValidateAsync(parameters, _cancellationToken);
    AssertIsValid(validatedToken);

    DateTime expiresOn = securityToken.ValidTo.AddMinutes(5); // NOTE(fpion): default TokenValidationParameters.ClockSkew
    _tokenBlacklist.Verify(x => x.GetBlacklistedAsync(It.Is<IEnumerable<string>>(y => y.SequenceEqual(tokenIds)), _cancellationToken), Times.Once);
    _tokenBlacklist.Verify(x => x.BlacklistAsync(tokenIds, expiresOn, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact(DisplayName = "ValidateAsync: it should throw SecurityTokenBlacklistedException when an identifier is blacklisted.")]
  public async Task ValidateAsync_it_should_throw_SecurityTokenBlacklistedException_when_an_identifier_is_blacklisted()
  {
    string[] tokenIds = [_tokenId];

    _tokenBlacklist.Setup(x => x.GetBlacklistedAsync(tokenIds, _cancellationToken)).ReturnsAsync(tokenIds);

    SecurityTokenDescriptor tokenDescriptor = new()
    {
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret)), SecurityAlgorithms.HmacSha256),
      Subject = _subject,
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    var exception = await Assert.ThrowsAsync<SecurityTokenBlacklistedException>(
      async () => await _tokenManager.ValidateAsync(token, _secret, _cancellationToken)
    );
    Assert.Equal(tokenIds, exception.BlacklistedIds);

    _tokenBlacklist.Verify(x => x.BlacklistAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact(DisplayName = "ValidateAsync: it should validate a token from parameters.")]
  public async Task ValidateAsync_it_should_validate_a_token_from_parameters()
  {
    string[] tokenIds = [_tokenId];
    _tokenBlacklist.Setup(x => x.GetBlacklistedAsync(tokenIds, _cancellationToken)).ReturnsAsync([]);

    SecurityTokenDescriptor tokenDescriptor = new()
    {
      Audience = "Audience",
      Issuer = "Issuer",
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret)), SecurityAlgorithms.HmacSha256),
      Subject = _subject,
      TokenType = "ID+JWT"
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    ValidateTokenParameters parameters = new(token, _secret)
    {
      ValidTypes = ["ID+JWT"],
      ValidAudiences = ["Audience"],
      ValidIssuers = ["Issuer"],
      Consume = false
    };
    ValidatedToken validatedToken = await _tokenManager.ValidateAsync(parameters, _cancellationToken);
    AssertIsValid(validatedToken);

    _tokenBlacklist.Verify(x => x.GetBlacklistedAsync(It.Is<IEnumerable<string>>(y => y.SequenceEqual(tokenIds)), _cancellationToken), Times.Once);
    _tokenBlacklist.Verify(x => x.BlacklistAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact(DisplayName = "ValidateAsync: it should validate a token with options.")]
  public async Task ValidateAsync_it_should_validate_a_token_with_options()
  {
    string[] tokenIds = [_tokenId];
    _tokenBlacklist.Setup(x => x.GetBlacklistedAsync(tokenIds, _cancellationToken)).ReturnsAsync([]);

    SecurityTokenDescriptor tokenDescriptor = new()
    {
      Audience = "Audience",
      Issuer = "Issuer",
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret)), SecurityAlgorithms.HmacSha256),
      Subject = _subject,
      TokenType = "ID+JWT"
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    ValidateTokenOptions options = new()
    {
      ValidTypes = ["ID+JWT"],
      ValidAudiences = ["Audience"],
      ValidIssuers = ["Issuer"],
      Consume = false
    };
    ValidatedToken validatedToken = await _tokenManager.ValidateAsync(token, _secret, options, _cancellationToken);
    AssertIsValid(validatedToken);

    _tokenBlacklist.Verify(x => x.GetBlacklistedAsync(It.Is<IEnumerable<string>>(y => y.SequenceEqual(tokenIds)), _cancellationToken), Times.Once);
    _tokenBlacklist.Verify(x => x.BlacklistAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact(DisplayName = "ValidateAsync: it should validate a token without options.")]
  public async Task ValidateAsync_it_should_validate_a_token_without_options()
  {
    string[] tokenIds = [_tokenId];
    _tokenBlacklist.Setup(x => x.GetBlacklistedAsync(tokenIds, _cancellationToken)).ReturnsAsync([]);

    SecurityTokenDescriptor tokenDescriptor = new()
    {
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret)), SecurityAlgorithms.HmacSha256),
      Subject = _subject
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    ValidatedToken validatedToken;

    validatedToken = await _tokenManager.ValidateAsync(token, _secret, _cancellationToken);
    AssertIsValid(validatedToken);

    validatedToken = await _tokenManager.ValidateAsync(token, _secret, options: null, _cancellationToken);
    AssertIsValid(validatedToken);

    _tokenBlacklist.Verify(x => x.GetBlacklistedAsync(It.Is<IEnumerable<string>>(y => y.SequenceEqual(tokenIds)), _cancellationToken), Times.Exactly(2));
    _tokenBlacklist.Verify(x => x.BlacklistAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  private void AssertIsValid(CreatedToken createdToken, CreateTokenOptions? options = null)
  {
    TokenValidationParameters validationParameters = new()
    {
      ClockSkew = TimeSpan.FromSeconds(0),
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret)),
      ValidateAudience = false,
      ValidateIssuer = false,
      ValidateIssuerSigningKey = true,
      ValidateLifetime = true
    };

    if (options != null)
    {
      validationParameters.ValidTypes = [options.Type];

      if (options.Audience != null)
      {
        validationParameters.ValidAudience = options.Audience;
        validationParameters.ValidateAudience = true;
      }
      if (options.Issuer != null)
      {
        validationParameters.ValidIssuer = options.Issuer;
        validationParameters.ValidateIssuer = true;
      }
    }

    ClaimsPrincipal principal = _tokenHandler.ValidateToken(createdToken.TokenString, validationParameters, out _);

    Dictionary<string, string> claims = principal.Claims.ToDictionary(x => x.Type, x => x.Value);
    foreach (Claim claim in _subject.Claims)
    {
      Assert.True(claims.ContainsKey(claim.Type));
      Assert.Equal(claim.Value, claims[claim.Type]);
    }

    if (options != null)
    {
      if (options.Audience != null)
      {
        Assert.True(claims.ContainsKey(Rfc7519ClaimNames.Audience));
        Assert.Equal(options.Audience, claims[Rfc7519ClaimNames.Audience]);
      }
      if (options.Issuer != null)
      {
        Assert.True(claims.ContainsKey(Rfc7519ClaimNames.Issuer));
        Assert.Equal(options.Issuer, claims[Rfc7519ClaimNames.Issuer]);
      }

      if (options.Expires.HasValue)
      {
        Claim claim = ClaimHelper.Create(Rfc7519ClaimNames.ExpirationTime, options.Expires.Value);
        Assert.True(claims.ContainsKey(claim.Type));
        Assert.Equal(claim.Value, claims[claim.Type]);
      }
      if (options.IssuedAt.HasValue)
      {
        Claim claim = ClaimHelper.Create(Rfc7519ClaimNames.IssuedAt, options.IssuedAt.Value);
        Assert.True(claims.ContainsKey(claim.Type));
        Assert.Equal(claim.Value, claims[claim.Type]);
      }
      if (options.NotBefore.HasValue)
      {
        Claim claim = ClaimHelper.Create(Rfc7519ClaimNames.NotBefore, options.NotBefore.Value);
        Assert.True(claims.ContainsKey(claim.Type));
        Assert.Equal(claim.Value, claims[claim.Type]);
      }
    }
  }

  private void AssertIsValid(ValidatedToken validatedToken)
  {
    Dictionary<string, string> claims = validatedToken.ClaimsPrincipal.Claims.ToDictionary(x => x.Type, x => x.Value);
    foreach (Claim claim in _subject.Claims)
    {
      Assert.True(claims.ContainsKey(claim.Type));
      Assert.Equal(claim.Value, claims[claim.Type]);
    }
  }
}
