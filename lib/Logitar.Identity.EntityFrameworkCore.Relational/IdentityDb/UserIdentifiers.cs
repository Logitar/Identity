using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

public static class UserIdentifiers
{
  public static readonly TableId Table = new(IdentityContext.Schema, nameof(IdentityContext.UserIdentifiers), alias: null);

  public static readonly ColumnId Key = new(nameof(UserIdentifierEntity.Key), Table);
  public static readonly ColumnId TenantId = new(nameof(UserIdentifierEntity.TenantId), Table);
  public static readonly ColumnId UserId = new(nameof(UserIdentifierEntity.UserId), Table);
  public static readonly ColumnId UserIdentifierId = new(nameof(UserIdentifierEntity.UserIdentifierId), Table);
  public static readonly ColumnId Value = new(nameof(UserIdentifierEntity.Value), Table);
}
