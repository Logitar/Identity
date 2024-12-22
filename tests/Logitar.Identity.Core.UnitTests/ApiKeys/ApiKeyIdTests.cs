using Logitar.EventSourcing;

namespace Logitar.Identity.Core.ApiKeys;

[Trait(Traits.Category, Categories.Unit)]
public class ApiKeyIdTests
{
  [Theory(DisplayName = "ctor: it should construct the correct ID from a stream ID.")]
  [InlineData(null)]
  [InlineData("TenantId")]
  public void Given_StreamId_When_ctor_Then_CorrectIdConstructed(string? tenantIdValue)
  {
    TenantId? tenantId = tenantIdValue == null ? null : new(tenantIdValue);
    EntityId entityId = EntityId.NewId();
    StreamId streamId = new(tenantId.HasValue ? string.Join(':', tenantId, entityId) : entityId.Value);

    ApiKeyId id = new(streamId);

    Assert.Equal(tenantId, id.TenantId);
    Assert.Equal(entityId, id.EntityId);
  }

  [Theory(DisplayName = "ctor: it should construct the correct ID from a tenant ID and an entity ID.")]
  [InlineData(null)]
  [InlineData("TenantId")]
  public void Given_TenantAndEntityId_When_ctor_Then_CorrectIdConstructed(string? tenantIdValue)
  {
    TenantId? tenantId = tenantIdValue == null ? null : new(tenantIdValue);
    EntityId entityId = EntityId.NewId();

    ApiKeyId id = new(tenantId, entityId);

    Assert.Equal(tenantId, id.TenantId);
    Assert.Equal(entityId, id.EntityId);
  }

  [Fact(DisplayName = "Equals: it should return false when the IDs are different.")]
  public void Given_DifferentIds_When_Equals_Then_FalseReturned()
  {
    ApiKeyId id1 = new(TenantId.NewId(), EntityId.NewId());
    ApiKeyId id2 = new(tenantId: null, id1.EntityId);
    Assert.False(id1.Equals(id2));
  }

  [Theory(DisplayName = "Equals: it should return false when the object do not have the same types.")]
  [InlineData(null)]
  [InlineData(123)]
  public void Given_DifferentTypes_When_Equals_Then_FalseReturned(object? value)
  {
    ApiKeyId id = ApiKeyId.NewId();
    Assert.False(id.Equals(value));
  }

  [Fact(DisplayName = "Equals: it should return true when the IDs are the same.")]
  public void Given_SameIds_When_Equals_Then_TrueReturned()
  {
    ApiKeyId id1 = new(TenantId.NewId(), EntityId.NewId());
    ApiKeyId id2 = new(id1.StreamId);
    Assert.True(id1.Equals(id1));
    Assert.True(id1.Equals(id2));
  }

  [Fact(DisplayName = "EqualOperator: it should return false when the IDs are different.")]
  public void Given_DifferentIds_When_EqualOperator_Then_FalseReturned()
  {
    ApiKeyId id1 = new(TenantId.NewId(), EntityId.NewId());
    ApiKeyId id2 = new(tenantId: null, id1.EntityId);
    Assert.False(id1 == id2);
  }

  [Fact(DisplayName = "EqualOperator: it should return true when the IDs are the same.")]
  public void Given_SameIds_When_EqualOperator_Then_TrueReturned()
  {
    ApiKeyId id1 = new(TenantId.NewId(), EntityId.NewId());
    ApiKeyId id2 = new(id1.StreamId);
    Assert.True(id1 == id2);
  }

  [Theory(DisplayName = "NewId: it should generate a new random ID with or without a tenant ID.")]
  [InlineData(null)]
  [InlineData("TenantId")]
  public void Given_TenantId_When_NewId_Then_NewRandomIdGenerated(string? tenantIdValue)
  {
    TenantId? tenantId = tenantIdValue == null ? null : new(tenantIdValue);

    ApiKeyId id = ApiKeyId.NewId(tenantId);

    Assert.Equal(tenantId, id.TenantId);
    Assert.NotEqual(Guid.Empty, id.EntityId.ToGuid());
  }

  [Theory(DisplayName = "GetHashCode: it should return the correct hash code.")]
  [InlineData(null)]
  [InlineData("TenantId")]
  public void Given_Id_When_GetHashCode_Then_CorrectHashCodeReturned(string? tenantIdValue)
  {
    TenantId? tenantId = tenantIdValue == null ? null : new(tenantIdValue);
    EntityId entityId = EntityId.NewId();

    ApiKeyId id = new(tenantId, entityId);

    Assert.Equal(id.Value.GetHashCode(), id.GetHashCode());
  }

  [Fact(DisplayName = "NotEqualOperator: it should return false when the IDs are the same.")]
  public void Given_SameIds_When_NotEqualOperator_Then_TrueReturned()
  {
    ApiKeyId id1 = new(TenantId.NewId(), EntityId.NewId());
    ApiKeyId id2 = new(id1.StreamId);
    Assert.False(id1 != id2);
  }

  [Fact(DisplayName = "NotEqualOperator: it should return true when the IDs are different.")]
  public void Given_DifferentIds_When_NotEqualOperator_Then_TrueReturned()
  {
    ApiKeyId id1 = new(TenantId.NewId(), EntityId.NewId());
    ApiKeyId id2 = new(tenantId: null, id1.EntityId);
    Assert.True(id1 != id2);
  }

  [Theory(DisplayName = "ToString: it should return the correct string representation.")]
  [InlineData(null)]
  [InlineData("TenantId")]
  public void Given_Id_When_ToString_Then_CorrectStringReturned(string? tenantIdValue)
  {
    TenantId? tenantId = tenantIdValue == null ? null : new(tenantIdValue);
    EntityId entityId = EntityId.NewId();

    ApiKeyId id = new(tenantId, entityId);

    Assert.Equal(id.Value, id.ToString());
  }
}
