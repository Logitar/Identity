using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public static class IdentityDb
{
  public static class Sessions
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Sessions));

    public static readonly ColumnId AggregateId = new(nameof(SessionEntity.AggregateId), Table);
    public static readonly ColumnId UserId = new(nameof(SessionEntity.UserId), Table);
  }

  public static class Users
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Users));

    public static readonly ColumnId AggregateId = new(nameof(UserEntity.AggregateId), Table);
    public static readonly ColumnId EmailAddressNormalized = new(nameof(UserEntity.EmailAddressNormalized), Table);
    public static readonly ColumnId TenantId = new(nameof(UserEntity.TenantId), Table);
    public static readonly ColumnId UniqueNameNormalized = new(nameof(UserEntity.UniqueNameNormalized), Table);
    public static readonly ColumnId UserId = new(nameof(UserEntity.UserId), Table);
  }
}
