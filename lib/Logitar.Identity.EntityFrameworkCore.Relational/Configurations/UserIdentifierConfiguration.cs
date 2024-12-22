using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public sealed class UserIdentifierConfiguration : IdentifierConfiguration<UserIdentifierEntity>, IEntityTypeConfiguration<UserIdentifierEntity>
{
  public override void Configure(EntityTypeBuilder<UserIdentifierEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(IdentityDb.UserIdentifiers.Table.Table ?? string.Empty, IdentityDb.UserIdentifiers.Table.Schema);
    builder.HasKey(x => x.UserIdentifierId);

    builder.HasIndex(x => new { x.UserId, x.Key }).IsUnique();
    builder.HasIndex(x => x.Key);
    builder.HasIndex(x => x.Value);

    builder.HasOne(x => x.User).WithMany(x => x.Identifiers)
      .HasPrincipalKey(x => x.UserId).HasForeignKey(x => x.UserId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
