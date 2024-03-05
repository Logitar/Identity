using Logitar.Identity.Domain.Roles;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

internal static class AssertRoles
{
  public static void AreEqual(RoleAggregate? role, RoleEntity? entity)
  {
    if (role == null || entity == null)
    {
      Assert.Null(role);
      Assert.Null(entity);
      return;
    }

    Assert.Equal(role.Version, entity.Version);
    Assert.Equal(role.CreatedBy.Value, entity.CreatedBy);
    Assertions.Equal(role.CreatedOn, entity.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(role.UpdatedBy.Value, entity.UpdatedBy);
    Assertions.Equal(role.UpdatedOn, entity.UpdatedOn, TimeSpan.FromSeconds(1));

    Assert.Equal(role.TenantId?.Value, entity.TenantId);
    Assert.Equal(role.UniqueName.Value, entity.UniqueName);
    Assert.Equal(role.DisplayName?.Value, entity.DisplayName);
    Assert.Equal(role.Description?.Value, entity.Description);

    Assert.Equal(role.CustomAttributes, entity.CustomAttributes);
  }
}
