using Bogus;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Tokens;
using Logitar.Security.Claims;
using Logitar.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Infrastructure.Tokens;

[Trait(Traits.Category, Categories.Unit)]
public class JsonWebTokenManagerTests
{
  private readonly Faker _faker = new();
  private readonly ClaimsIdentity _subject = new();
  private readonly string _secret = RandomStringGenerator.GetString(256 / 8);

  private readonly JwtSecurityTokenHandler _tokenHandler = new();
  private readonly JsonWebTokenManager _tokenManager;

  public JsonWebTokenManagerTests()
  {
    DateTime now = DateTime.Now;
    _subject.AddClaim(new(Rfc7519ClaimNames.Subject, $"UserId:{Guid.NewGuid()}"));
    _subject.AddClaim(new(Rfc7519ClaimNames.TokenId, $"TokenId:{Guid.NewGuid()}"));
    _subject.AddClaim(new(Rfc7519ClaimNames.Username, _faker.Person.UserName));
    _subject.AddClaim(new(Rfc7519ClaimNames.EmailAddress, _faker.Person.Email));
    _subject.AddClaim(new(Rfc7519ClaimNames.IsEmailVerified, bool.TrueString.ToLower()));
    _subject.AddClaim(new(Rfc7519ClaimNames.PhoneNumber, _faker.Person.Phone));
    _subject.AddClaim(new(Rfc7519ClaimNames.IsPhoneVerified, bool.FalseString.ToLower()));
    _subject.AddClaim(new(Rfc7519ClaimNames.FirstName, _faker.Person.FirstName));
    _subject.AddClaim(new(Rfc7519ClaimNames.LastName, _faker.Person.LastName));
    _subject.AddClaim(new(Rfc7519ClaimNames.FullName, _faker.Person.FullName));
    _subject.AddClaim(ClaimHelper.Create(Rfc7519ClaimNames.Birthdate, _faker.Person.DateOfBirth));
    _subject.AddClaim(new(Rfc7519ClaimNames.Gender, _faker.Person.Gender.ToString().ToLower()));
    _subject.AddClaim(new(Rfc7519ClaimNames.Locale, _faker.Locale));
    _subject.AddClaim(new(Rfc7519ClaimNames.Picture, _faker.Person.Avatar));
    _subject.AddClaim(ClaimHelper.Create(Rfc7519ClaimNames.UpdatedAt, now));
    _subject.AddClaim(ClaimHelper.Create(Rfc7519ClaimNames.AuthenticationTime, DateTime.Now));
    _subject.AddClaim(new(Rfc7519ClaimNames.SessionId, $"SessionId:{Guid.NewGuid()}"));
    _subject.AddClaim(new(Rfc7519ClaimNames.Roles, "manage_users manage_sessions"));

    _tokenManager = new(_tokenHandler);
  }

  [Fact(DisplayName = "Create: it should create the correct token from parameters.")]
  public void Create_it_should_create_the_correct_token_from_parameters()
  {
    DateTime now = DateTime.Now;
    CreateTokenParameters parameters = new(_subject, _secret)
    {
      Type = "ID+JWT",
      Audience = "test_audience",
      Issuer = "test_issuer",
      Expires = now.AddMinutes(15),
      IssuedAt = now.AddSeconds(-15).ToUniversalTime(),
      NotBefore = new(now.ToUniversalTime().Ticks, DateTimeKind.Unspecified)
    };
    CreatedToken createdToken = _tokenManager.Create(parameters);
    AssertIsValid(createdToken, parameters);
  }

  [Fact(DisplayName = "Create: it should create the correct token with options.")]
  public void Create_it_should_create_the_correct_token_with_options()
  {
    DateTime now = DateTime.Now;
    CreateTokenOptions options = new()
    {
      Type = "ID+JWT",
      Audience = "test_audience",
      Issuer = "test_issuer",
      Expires = now.AddMinutes(15),
      IssuedAt = now.AddSeconds(-15).ToUniversalTime(),
      NotBefore = new(now.ToUniversalTime().Ticks, DateTimeKind.Unspecified)
    };
    CreatedToken createdToken = _tokenManager.Create(_subject, _secret, options);
    AssertIsValid(createdToken, options);
  }

  [Fact(DisplayName = "Create: it should create the correct token without options.")]
  public void Create_it_should_create_the_correct_token_without_options()
  {
    CreatedToken createdToken = _tokenManager.Create(_subject, _secret, options: null);
    AssertIsValid(createdToken);
  }

  [Fact(DisplayName = "ctor: it should construct the correct instance.")]
  public void ctor_it_should_construct_the_correct_instance()
  {
    Assert.Empty(_tokenHandler.InboundClaimTypeMap);
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
        Claim claim = ClaimHelper.Create(Rfc7519ClaimNames.NotBefore, DateTime.SpecifyKind(options.NotBefore.Value, DateTimeKind.Utc)); // TODO(fpion): should be in ClaimHelper.Create method
        Assert.True(claims.ContainsKey(claim.Type));
        Assert.Equal(claim.Value, claims[claim.Type]);
      }
    }
  }
}
