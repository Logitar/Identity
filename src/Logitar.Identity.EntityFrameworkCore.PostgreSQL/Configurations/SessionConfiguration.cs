using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Configurations;

/// <summary>
/// The model configuration used to configure the <see cref="SessionEntity"/> class.
/// </summary>
internal class SessionConfiguration : AggregateConfiguration<SessionEntity>, IEntityTypeConfiguration<SessionEntity>
{
  /// <summary>
  /// Configures the database model using the specified type builder.
  /// </summary>
  /// <param name="builder">The type builder.</param>
  public override void Configure(EntityTypeBuilder<SessionEntity> builder)
  {
    base.Configure(builder);

    builder.HasKey(x => x.SessionId);

    builder.HasOne(x => x.User).WithMany(x => x.Sessions).OnDelete(DeleteBehavior.Restrict);

    builder.HasIndex(x => x.IsPersistent);
    builder.HasIndex(x => x.SignedOutById);
    builder.HasIndex(x => x.SignedOutOn);
    builder.HasIndex(x => x.IsActive);

    builder.Property(x => x.KeyHash).HasMaxLength(ushort.MaxValue);
    builder.Property(x => x.IsPersistent).HasDefaultValue(false);
    builder.Property(x => x.SignedOutById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.SignedOutBy).HasColumnType("jsonb");
    builder.Property(x => x.IsActive).HasDefaultValue(false);
    builder.Property(x => x.CustomAttributes).HasColumnType("jsonb");
  }
}
