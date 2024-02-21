using Bogus;
using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Passwords;

[Trait(Traits.Category, Categories.Unit)]
public class OneTimePasswordIdTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "ctor: it should create an identifier from a Guid.")]
  public void ctor_it_should_create_an_identifier_from_a_Guid()
  {
    Guid guid = Guid.NewGuid();
    OneTimePasswordId id = new(guid);
    string expected = new AggregateId(guid).Value;
    Assert.Equal(expected, id.Value);
  }

  [Theory(DisplayName = "ctor: it should create a new One-Time Password identifier.")]
  [InlineData("86f2b595-a803-40c5-b270-f33ea53620b0")]
  [InlineData("  admin  ")]
  public void ctor_it_should_create_a_new_One_Time_Password_identifier(string value)
  {
    OneTimePasswordId oneTimePasswordId = new(value);
    Assert.Equal(value.Trim(), oneTimePasswordId.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(OneTimePasswordId);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new OneTimePasswordId(value, propertyName));
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
    string propertyName = nameof(OneTimePasswordId);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new OneTimePasswordId(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal("MaximumLengthValidator", e.ErrorCode);
      Assert.Equal(propertyName, e.PropertyName);
    });
  }

  [Fact(DisplayName = "NewId: it should create a new One-Time Password ID.")]
  public void NewId_it_should_create_a_new_One_Time_Password_Id()
  {
    OneTimePasswordId id = OneTimePasswordId.NewId();
    Assert.Equal(id.AggregateId.Value, id.Value);
  }

  [Fact(DisplayName = "ToGuid: it should convert the identifier to a Guid.")]
  public void ToGuid_it_should_convert_the_identifier_to_a_Guid()
  {
    Guid guid = Guid.NewGuid();
    OneTimePasswordId id = new(new AggregateId(guid));
    Assert.Equal(guid, id.ToGuid());
  }

  [Theory(DisplayName = "TryCreate: it should return a One-Time Password identifier when the value is not empty.")]
  [InlineData("85107c8e-0730-4dc1-99f4-4008d0ea7688")]
  [InlineData("  admin  ")]
  public void TryCreate_it_should_return_a_One_Time_Password_when_the_value_is_not_empty(string value)
  {
    OneTimePasswordId? oneTimePasswordId = OneTimePasswordId.TryCreate(value);
    Assert.NotNull(oneTimePasswordId);
    Assert.Equal(value.Trim(), oneTimePasswordId.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(OneTimePasswordId.TryCreate(value));
  }
}
