using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

public static class ApiKeys
{
  public static readonly TableId Table = new(IdentityContext.Schema, nameof(IdentityContext.ApiKeys), alias: null);

  public static readonly ColumnId CreatedBy = new(nameof(ApiKeyEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(ApiKeyEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(ApiKeyEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(ApiKeyEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(ApiKeyEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(ApiKeyEntity.Version), Table);

  public static readonly ColumnId ApiKeyId = new(nameof(ApiKeyEntity.ApiKeyId), Table);
  public static readonly ColumnId AuthenticatedOn = new(nameof(ApiKeyEntity.AuthenticatedOn), Table);
  public static readonly ColumnId CustomAttributes = new(nameof(ApiKeyEntity.CustomAttributes), Table);
  public static readonly ColumnId Description = new(nameof(ApiKeyEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(ApiKeyEntity.DisplayName), Table);
  public static readonly ColumnId EntityId = new(nameof(ApiKeyEntity.EntityId), Table);
  public static readonly ColumnId ExpiresOn = new(nameof(ApiKeyEntity.ExpiresOn), Table);
  public static readonly ColumnId SecretHash = new(nameof(ApiKeyEntity.SecretHash), Table);
  public static readonly ColumnId TenantId = new(nameof(ApiKeyEntity.TenantId), Table);
}
