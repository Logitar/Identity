using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

public static class Roles
{
  public static readonly TableId Table = new(IdentityContext.Schema, nameof(IdentityContext.Roles), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(RoleEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(RoleEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(RoleEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(RoleEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(RoleEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(RoleEntity.Version), Table);

  public static readonly ColumnId CustomAttributes = new(nameof(RoleEntity.CustomAttributes), Table);
  public static readonly ColumnId Description = new(nameof(RoleEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(RoleEntity.DisplayName), Table);
  public static readonly ColumnId EntityId = new(nameof(RoleEntity.EntityId), Table);
  public static readonly ColumnId RoleId = new(nameof(RoleEntity.RoleId), Table);
  public static readonly ColumnId TenantId = new(nameof(RoleEntity.TenantId), Table);
  public static readonly ColumnId UniqueName = new(nameof(RoleEntity.UniqueName), Table);
  public static readonly ColumnId UniqueNameNormalized = new(nameof(RoleEntity.UniqueNameNormalized), Table);
}
