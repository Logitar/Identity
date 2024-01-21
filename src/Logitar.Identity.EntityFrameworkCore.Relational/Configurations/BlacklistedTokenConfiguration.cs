using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class BlacklistedTokenConfiguration : IEntityTypeConfiguration<BlacklistedTokenEntity>
{
  public void Configure(EntityTypeBuilder<BlacklistedTokenEntity> builder)
  {
    builder.ToTable(nameof(IdentityContext.TokenBlacklist));
    builder.HasKey(x => x.BlacklistedTokenId);

    builder.HasIndex(x => x.TokenId).IsUnique();
    builder.HasIndex(x => x.ExpiresOn);

    builder.Property(x => x.TokenId).HasMaxLength(byte.MaxValue);
  }
}
