using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

public static class UserRoles
{
  public static readonly TableId Table = new(IdentityContext.Schema, nameof(IdentityContext.UserRoles), alias: null);

  public static readonly ColumnId UserId = new(nameof(UserRoleEntity.UserId), Table);
  public static readonly ColumnId RoleId = new(nameof(UserRoleEntity.RoleId), Table);
}
