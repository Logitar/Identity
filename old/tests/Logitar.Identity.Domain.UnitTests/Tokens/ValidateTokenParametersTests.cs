namespace Logitar.Identity.Domain.Tokens;

[Trait(Traits.Category, Categories.Unit)]
public class ValidateTokenParametersTests
{
  private const string Secret = "S3cR3+!*";
  private const string Token = "token";

  [Fact(DisplayName = "ctor: it should construct the correct parameters with options.")]
  public void ctor_it_should_construct_the_correct_parameters_with_options()
  {
    DateTime now = DateTime.UtcNow;
    ValidateTokenOptions options = new()
    {
      ValidTypes = ["ID+JWT"],
      ValidAudiences = ["Audience"],
      ValidIssuers = ["Issuer"],
      Consume = true
    };

    ValidateTokenParameters parameters = new(Token, Secret, options);
    Assert.Equal(Token, parameters.Token);
    Assert.Equal(Secret, parameters.Secret);
    Assert.Equal(options.ValidTypes, parameters.ValidTypes);
    Assert.Equal(options.ValidAudiences, parameters.ValidAudiences);
    Assert.Equal(options.ValidIssuers, parameters.ValidIssuers);
    Assert.Equal(options.Consume, parameters.Consume);
  }

  [Fact(DisplayName = "ctor: it should construct the correct parameters.")]
  public void ctor_it_should_construct_the_correct_parameters()
  {
    ClaimsIdentity subject = new();
    ValidateTokenParameters parameters = new(Token, Secret);
    Assert.Equal(Token, parameters.Token);
    Assert.Equal(Secret, parameters.Secret);
  }
}
