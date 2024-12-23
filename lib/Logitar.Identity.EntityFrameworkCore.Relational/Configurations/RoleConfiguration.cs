using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public sealed class RoleConfiguration : AggregateConfiguration<RoleEntity>, IEntityTypeConfiguration<RoleEntity>
{
  public override void Configure(EntityTypeBuilder<RoleEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(IdentityDb.Roles.Table.Table ?? string.Empty, IdentityDb.Roles.Table.Schema);
    builder.HasKey(x => x.RoleId);

    builder.HasIndex(x => new { x.TenantId, x.EntityId }).IsUnique();
    builder.HasIndex(x => x.EntityId);
    builder.HasIndex(x => x.UniqueName);
    builder.HasIndex(x => new { x.TenantId, x.UniqueNameNormalized }).IsUnique();
    builder.HasIndex(x => x.DisplayName);

    builder.Property(x => x.TenantId).HasMaxLength(StreamId.MaximumLength);
    builder.Property(x => x.EntityId).HasMaxLength(StreamId.MaximumLength);
    builder.Property(x => x.UniqueName).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
  }
}
