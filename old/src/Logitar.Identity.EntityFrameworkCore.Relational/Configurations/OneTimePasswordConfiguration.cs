using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class OneTimePasswordConfiguration : AggregateConfiguration<OneTimePasswordEntity>, IEntityTypeConfiguration<OneTimePasswordEntity>
{
  public override void Configure(EntityTypeBuilder<OneTimePasswordEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(IdentityContext.OneTimePasswords));
    builder.HasKey(x => x.OneTimePasswordId);

    builder.HasIndex(x => x.TenantId);
    builder.HasIndex(x => x.ExpiresOn);
    builder.HasIndex(x => x.MaximumAttempts);
    builder.HasIndex(x => x.AttemptCount);
    builder.HasIndex(x => x.HasValidationSucceeded);

    builder.Ignore(x => x.CustomAttributes);

    builder.Property(x => x.TenantId).HasMaxLength(AggregateId.MaximumLength);
    builder.Property(x => x.PasswordHash).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.CustomAttributesSerialized).HasColumnName(nameof(OneTimePasswordEntity.CustomAttributes));
  }
}
