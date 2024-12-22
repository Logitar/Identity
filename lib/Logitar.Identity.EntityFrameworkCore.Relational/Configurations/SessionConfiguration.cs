using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public sealed class SessionConfiguration : AggregateConfiguration<SessionEntity>, IEntityTypeConfiguration<SessionEntity>
{
  public override void Configure(EntityTypeBuilder<SessionEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(IdentityDb.Sessions.Table.Table ?? string.Empty, IdentityDb.Sessions.Table.Schema);
    builder.HasKey(x => x.SessionId);

    builder.HasIndex(x => new { x.TenantId, x.EntityId }).IsUnique();
    builder.HasIndex(x => x.EntityId);
    builder.HasIndex(x => x.IsPersistent);
    builder.HasIndex(x => x.SignedOutBy);
    builder.HasIndex(x => x.SignedOutOn);
    builder.HasIndex(x => x.IsActive);

    builder.Property(x => x.TenantId).HasMaxLength(StreamId.MaximumLength);
    builder.Property(x => x.EntityId).HasMaxLength(StreamId.MaximumLength);
    builder.Property(x => x.SecretHash).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.SignedOutBy).HasMaxLength(ActorId.MaximumLength);

    builder.HasOne(x => x.User).WithMany(x => x.Sessions)
      .HasPrincipalKey(x => x.UserId).HasForeignKey(x => x.UserId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
