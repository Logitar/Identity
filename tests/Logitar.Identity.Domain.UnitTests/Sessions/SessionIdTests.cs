using Bogus;
using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Sessions;

[Trait(Traits.Category, Categories.Unit)]
public class SessionIdTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should create a new session identifier.")]
  [InlineData("8b0a5032-f81f-4cef-99a6-de119025e379")]
  [InlineData("  702f0d94-a99c-4279-bc18-1ecc5762cf1d  ")]
  public void ctor_it_should_create_a_new_display_name(string value)
  {
    SessionId sessionId = new(value);
    Assert.Equal(value.Trim(), sessionId.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(SessionId);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new SessionId(value, propertyName));
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
    string propertyName = nameof(SessionId);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new SessionId(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal("MaximumLengthValidator", e.ErrorCode);
      Assert.Equal(propertyName, e.PropertyName);
    });
  }

  [Fact(DisplayName = "NewId: it should create a new session ID.")]
  public void NewId_it_should_create_a_new_session_Id()
  {
    SessionId id = SessionId.NewId();
    Assert.Equal(id.AggregateId.Value, id.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return a session identifier when the value is not empty.")]
  [InlineData("795d605e-258b-432a-bd83-be470d5d240d")]
  [InlineData("  2e7c19db-cda2-46df-a62f-81c8aeee83c6  ")]
  public void TryCreate_it_should_return_a_display_name_when_the_value_is_not_empty(string value)
  {
    SessionId? sessionId = SessionId.TryCreate(value);
    Assert.NotNull(sessionId);
    Assert.Equal(value.Trim(), sessionId.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(SessionId.TryCreate(value));
  }
}
