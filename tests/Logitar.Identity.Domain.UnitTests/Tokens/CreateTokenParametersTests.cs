namespace Logitar.Identity.Domain.Tokens;

[Trait(Traits.Category, Categories.Unit)]
public class CreateTokenParametersTests
{
  [Fact(DisplayName = "ctor: it should construct the correct parameters with options.")]
  public void ctor_it_should_construct_the_correct_parameters_with_options()
  {
    ClaimsIdentity subject = new();
    string secret = "S3cR3+!*";

    DateTime now = DateTime.UtcNow;
    CreateTokenOptions options = new()
    {
      Audience = "Audience",
      Issuer = "Issuer",
      IssuedAt = now,
      NotBefore = now.AddMinutes(1),
      Expires = now.AddHours(1)
    };

    CreateTokenParameters parameters = new(subject, secret, options);
    Assert.Same(subject, parameters.Subject);
    Assert.Equal(secret, parameters.Secret);
    Assert.Equal(options.Type, parameters.Type);
    Assert.Equal(options.SigningAlgorithm, parameters.SigningAlgorithm);
    Assert.Equal(options.Audience, parameters.Audience);
    Assert.Equal(options.Issuer, parameters.Issuer);
    Assert.Equal(options.Expires, parameters.Expires);
    Assert.Equal(options.IssuedAt, parameters.IssuedAt);
    Assert.Equal(options.NotBefore, parameters.NotBefore);
  }

  [Fact(DisplayName = "ctor: it should construct the correct parameters.")]
  public void ctor_it_should_construct_the_correct_parameters()
  {
    ClaimsIdentity subject = new();
    string secret = "S3cR3+!*";
    CreateTokenParameters parameters = new(subject, secret);
    Assert.Same(subject, parameters.Subject);
    Assert.Equal(secret, parameters.Secret);
  }
}
