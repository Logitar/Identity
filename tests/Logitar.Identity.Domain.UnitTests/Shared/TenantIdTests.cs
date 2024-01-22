using Bogus;
using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Shared;

[Trait(Traits.Category, Categories.Unit)]
public class TenantIdTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ctor: it should create a new tenant identifier.")]
  [InlineData("59e2fc4b-f4e4-4052-a3b0-2f4375964149")]
  [InlineData("  59e2fc4b-f4e4-4052-a3b0-2f4375964149  ")]
  public void ctor_it_should_create_a_new_tenant_identifier(string value)
  {
    TenantId tenantId = new(value);
    Assert.Equal(value.Trim(), tenantId.Value);
  }

  [Theory(DisplayName = "ctor: it should throw ValidationException when the value is empty.")]
  [InlineData("")]
  [InlineData("  ")]
  public void ctor_it_should_throw_ValidationException_when_the_value_is_empty(string value)
  {
    string propertyName = nameof(TenantId);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new TenantId(value, propertyName));
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
    string propertyName = nameof(TenantId);

    var exception = Assert.Throws<FluentValidation.ValidationException>(() => new TenantId(value, propertyName));
    Assert.All(exception.Errors, e =>
    {
      Assert.Equal("MaximumLengthValidator", e.ErrorCode);
      Assert.Equal(propertyName, e.PropertyName);
    });
  }

  [Theory(DisplayName = "TryCreate: it should return a tenant identifier when the value is not empty.")]
  [InlineData("ede716c6-820a-4276-a3f9-d65645db7538")]
  [InlineData("  test  ")]
  public void TryCreate_it_should_return_a_tenant_identifier_when_the_value_is_not_empty(string value)
  {
    TenantId? tenantId = TenantId.TryCreate(value);
    Assert.NotNull(tenantId);
    Assert.Equal(value.Trim(), tenantId.Value);
  }

  [Theory(DisplayName = "TryCreate: it should return null when the value is null or whitespace.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void TryCreate_it_should_return_null_when_the_value_is_null_or_whitespace(string? value)
  {
    Assert.Null(TenantId.TryCreate(value));
  }
}
