using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

public static class CustomAttributes
{
  public static readonly TableId Table = new(IdentityContext.Schema, nameof(IdentityContext.CustomAttributes), alias: null);

  public static readonly ColumnId CustomAttributeId = new(nameof(CustomAttributeEntity.CustomAttributeId), Table);
  public static readonly ColumnId EntityId = new(nameof(CustomAttributeEntity.EntityId), Table);
  public static readonly ColumnId EntityType = new(nameof(CustomAttributeEntity.EntityType), Table);
  public static readonly ColumnId Key = new(nameof(CustomAttributeEntity.Key), Table);
  public static readonly ColumnId Value = new(nameof(CustomAttributeEntity.Value), Table);
  public static readonly ColumnId ValueShortened = new(nameof(CustomAttributeEntity.ValueShortened), Table);
}
