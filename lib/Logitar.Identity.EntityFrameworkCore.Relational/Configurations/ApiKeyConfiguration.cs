using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public sealed class ApiKeyConfiguration : AggregateConfiguration<ApiKeyEntity>, IEntityTypeConfiguration<ApiKeyEntity>
{
  public override void Configure(EntityTypeBuilder<ApiKeyEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(IdentityDb.ApiKeys.Table.Table ?? string.Empty, IdentityDb.ApiKeys.Table.Schema);
    builder.HasKey(x => x.ApiKeyId);

    builder.HasIndex(x => new { x.TenantId, x.EntityId }).IsUnique();
    builder.HasIndex(x => x.EntityId);
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.ExpiresOn);
    builder.HasIndex(x => x.AuthenticatedOn);

    builder.Property(x => x.SecretHash).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.TenantId).HasMaxLength(StreamId.MaximumLength);
    builder.Property(x => x.EntityId).HasMaxLength(StreamId.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);

    builder.HasMany(x => x.Roles).WithMany(x => x.ApiKeys).UsingEntity<ApiKeyRoleEntity>(joinBuilder =>
    {
      joinBuilder.ToTable(IdentityDb.ApiKeyRoles.Table.Table ?? string.Empty, IdentityDb.ApiKeyRoles.Table.Schema);
      joinBuilder.HasKey(x => new { x.ApiKeyId, x.RoleId });
    });
  }
}
