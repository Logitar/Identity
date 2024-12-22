using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public sealed class OneTimePasswordConfiguration : AggregateConfiguration<OneTimePasswordEntity>, IEntityTypeConfiguration<OneTimePasswordEntity>
{
  public override void Configure(EntityTypeBuilder<OneTimePasswordEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(IdentityDb.OneTimePasswords.Table.Table ?? string.Empty, IdentityDb.OneTimePasswords.Table.Schema);
    builder.HasKey(x => x.OneTimePasswordId);

    builder.HasIndex(x => new { x.TenantId, x.EntityId }).IsUnique();
    builder.HasIndex(x => x.EntityId);
    builder.HasIndex(x => x.ExpiresOn);
    builder.HasIndex(x => x.MaximumAttempts);
    builder.HasIndex(x => x.AttemptCount);
    builder.HasIndex(x => x.HasValidationSucceeded);

    builder.Property(x => x.TenantId).HasMaxLength(StreamId.MaximumLength);
    builder.Property(x => x.EntityId).HasMaxLength(StreamId.MaximumLength);
    builder.Property(x => x.PasswordHash).HasMaxLength(byte.MaxValue);
  }
}
