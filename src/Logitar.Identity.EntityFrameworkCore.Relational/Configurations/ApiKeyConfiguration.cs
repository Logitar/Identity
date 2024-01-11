using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class ApiKeyConfiguration : AggregateConfiguration<ApiKeyEntity>, IEntityTypeConfiguration<ApiKeyEntity>
{
  public override void Configure(EntityTypeBuilder<ApiKeyEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(IdentityContext.ApiKeys));
    builder.HasKey(x => x.ApiKeyId);

    builder.HasIndex(x => x.TenantId);
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.ExpiresOn);
    builder.HasIndex(x => x.AuthenticatedOn);

    builder.Ignore(x => x.CustomAttributes);

    builder.Property(x => x.SecretHash).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.TenantId).HasMaxLength(AggregateId.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayNameUnit.MaximumLength);
    builder.Property(x => x.CustomAttributesSerialized).HasColumnName(nameof(ApiKeyEntity.CustomAttributes));

    builder.HasMany(x => x.Roles).WithMany(x => x.ApiKeys).UsingEntity<ApiKeyRoleEntity>(joinBuilder =>
    {
      joinBuilder.ToTable(nameof(IdentityContext.ApiKeyRoles));
      joinBuilder.HasKey(x => new { x.ApiKeyId, x.RoleId });
    });
  }
}
