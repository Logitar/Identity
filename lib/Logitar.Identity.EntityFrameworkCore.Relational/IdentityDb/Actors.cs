using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

public static class Actors
{
  public static readonly TableId Table = new(IdentityContext.Schema, nameof(IdentityContext.Actors), alias: null);

  public static readonly ColumnId ActorId = new(nameof(ActorEntity.ActorId), Table);
  public static readonly ColumnId DisplayName = new(nameof(ActorEntity.DisplayName), Table);
  public static readonly ColumnId EmailAddress = new(nameof(ActorEntity.EmailAddress), Table);
  public static readonly ColumnId Id = new(nameof(ActorEntity.Id), Table);
  public static readonly ColumnId IsDeleted = new(nameof(ActorEntity.IsDeleted), Table);
  public static readonly ColumnId PictureUrl = new(nameof(ActorEntity.PictureUrl), Table);
  public static readonly ColumnId Type = new(nameof(ActorEntity.Type), Table);
}
