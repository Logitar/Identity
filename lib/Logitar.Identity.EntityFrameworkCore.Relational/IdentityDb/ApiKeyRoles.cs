using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

public static class ApiKeyRoles
{
  public static readonly TableId Table = new(IdentityContext.Schema, nameof(IdentityContext.ApiKeyRoles), alias: null);

  public static readonly ColumnId ApiKeyId = new(nameof(ApiKeyRoleEntity.ApiKeyId), Table);
  public static readonly ColumnId RoleId = new(nameof(ApiKeyRoleEntity.RoleId), Table);
}
