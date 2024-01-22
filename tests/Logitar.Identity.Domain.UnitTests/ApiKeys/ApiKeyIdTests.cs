using Bogus;
using Logitar.EventSourcing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Logitar.Identity.Domain.ApiKeys;

[Trait(Traits.Category, Categories.Unit)]
public class ApiKeyIdTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should create a new API key identifier.")]
  [InlineData("64192609-6ad1-4f54-a8f0-a44372f229c8")]
  [InlineData("  bbea05b5-75c0-4ab5-8572-6d0dee8aa046  ")]
  public void ctor_it_should_create_a_new_Api_key_identifier(string value)
  {
    ApiKeyId apiKeyId = new(value);
    Assert.Equal(value.Trim(), apiKeyId.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(ApiKeyId);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new ApiKeyId(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal(propertyName, e.PropertyName);
      Assert.Equal("NotEmptyValidator", e.ErrorCode);
    });
  }

  [Fact(DisplayName = "ctor: it should throw ValidationException when the value is too long.")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_too_long()
  {
    string value = _faker.Random.String(AggregateId.MaximumLength + 1, minChar: 'A', maxChar: 'Z');
    string propertyName = nameof(ApiKeyId);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new ApiKeyId(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal("MaximumLengthValidator", e.ErrorCode);
      Assert.Equal(propertyName, e.PropertyName);
    });
  }

  [Fact(DisplayName = "NewId: it should create a new API key ID.")]
  public void NewId_it_should_create_a_new_Api_key_Id()
  {
    ApiKeyId id = ApiKeyId.NewId();
    Assert.Equal(id.AggregateId.Value, id.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return an API key identifier when the value is not empty.")]
  [InlineData("268f59a5-eaa4-4b04-9d96-d1bbaa453a19")]
  [InlineData("  0aee1e54-3f7d-4b87-a82e-d2e7924c46bc  ")]
  public void TryCreate_it_should_return_an_Api_key_identifier_when_the_value_is_not_empty(string value)
  {
    ApiKeyId? apiKeyId = ApiKeyId.TryCreate(value);
    Assert.NotNull(apiKeyId);
    Assert.Equal(value.Trim(), apiKeyId.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(ApiKeyId.TryCreate(value));
  }
}
