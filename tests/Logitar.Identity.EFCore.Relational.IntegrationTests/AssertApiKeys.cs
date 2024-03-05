using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

internal static class AssertApiKeys
{
  public static void AreEqual(ApiKeyAggregate? apiKey, ApiKeyEntity? entity)
  {
    if (apiKey == null || entity == null)
    {
      Assert.Null(apiKey);
      Assert.Null(entity);
      return;
    }

    Assert.Equal(apiKey.Version, entity.Version);
    Assert.Equal(apiKey.CreatedBy.Value, entity.CreatedBy);
    Assertions.Equal(apiKey.CreatedOn, entity.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(apiKey.UpdatedBy.Value, entity.UpdatedBy);
    Assertions.Equal(apiKey.UpdatedOn, entity.UpdatedOn, TimeSpan.FromSeconds(1));

    Assert.Equal(apiKey.TenantId?.Value, entity.TenantId);
    Assert.NotEmpty(entity.SecretHash);
    Assert.Equal(apiKey.DisplayName.Value, entity.DisplayName);
    Assert.Equal(apiKey.Description?.Value, entity.Description);
    Assertions.Equal(apiKey.ExpiresOn?.ToUniversalTime(), entity.ExpiresOn, TimeSpan.FromSeconds(1));
    Assertions.Equal(apiKey.AuthenticatedOn?.ToUniversalTime(), entity.AuthenticatedOn, TimeSpan.FromSeconds(1));

    Assert.Equal(apiKey.CustomAttributes, entity.CustomAttributes);

    foreach (RoleId roleId in apiKey.Roles)
    {
      Assert.Contains(entity.Roles, role => role.AggregateId == roleId.Value);
    }
  }

  public static void AreEquivalent(ApiKeyEntity? apiKey, ActorEntity? actor)
  {
    if (apiKey == null || actor == null)
    {
      Assert.Null(apiKey);
      Assert.Null(actor);
      return;
    }

    Assert.Equal(apiKey.AggregateId, actor.Id);
    Assert.Equal(ActorType.ApiKey, actor.Type);
    Assert.False(actor.IsDeleted);
    Assert.Equal(apiKey.DisplayName, actor.DisplayName);
    Assert.Null(actor.EmailAddress);
    Assert.Null(actor.PictureUrl);
  }
}
