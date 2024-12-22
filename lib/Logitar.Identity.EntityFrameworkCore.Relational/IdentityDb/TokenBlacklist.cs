using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

public static class TokenBlacklist
{
  public static readonly TableId Table = new(IdentityContext.Schema, nameof(IdentityContext.TokenBlacklist), alias: null);

  public static readonly ColumnId BlacklistedTokenId = new(nameof(BlacklistedTokenEntity.BlacklistedTokenId), Table);
  public static readonly ColumnId ExpiresOn = new(nameof(BlacklistedTokenEntity.ExpiresOn), Table);
  public static readonly ColumnId TokenId = new(nameof(BlacklistedTokenEntity.TokenId), Table);
}
