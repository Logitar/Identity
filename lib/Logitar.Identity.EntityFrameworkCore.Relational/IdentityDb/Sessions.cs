using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

public static class Sessions
{
  public static readonly TableId Table = new(IdentityContext.Schema, nameof(IdentityContext.Sessions), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(SessionEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(SessionEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(SessionEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(SessionEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(SessionEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(SessionEntity.Version), Table);

  public static readonly ColumnId CustomAttributes = new(nameof(SessionEntity.CustomAttributes), Table);
  public static readonly ColumnId EntityId = new(nameof(SessionEntity.EntityId), Table);
  public static readonly ColumnId IsActive = new(nameof(SessionEntity.IsActive), Table);
  public static readonly ColumnId IsPersistent = new(nameof(SessionEntity.IsPersistent), Table);
  public static readonly ColumnId SecretHash = new(nameof(SessionEntity.SecretHash), Table);
  public static readonly ColumnId SessionId = new(nameof(SessionEntity.SessionId), Table);
  public static readonly ColumnId SignedOutBy = new(nameof(SessionEntity.SignedOutBy), Table);
  public static readonly ColumnId SignedOutOn = new(nameof(SessionEntity.SignedOutOn), Table);
  public static readonly ColumnId TenantId = new(nameof(SessionEntity.TenantId), Table);
  public static readonly ColumnId UserId = new(nameof(SessionEntity.UserId), Table);
}
