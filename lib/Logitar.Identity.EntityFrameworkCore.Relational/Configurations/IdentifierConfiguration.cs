using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public abstract class IdentifierConfiguration<T> where T : IdentifierEntity
{
  public virtual void Configure(EntityTypeBuilder<T> builder)
  {
    builder.HasIndex(x => new { x.TenantId, x.Key, x.Value }).IsUnique();
    builder.HasIndex(x => x.Key);
    builder.HasIndex(x => x.Value);

    builder.Property(x => x.TenantId).HasMaxLength(StreamId.MaximumLength);
    builder.Property(x => x.Key).HasMaxLength(Identifier.MaximumLength);
    builder.Property(x => x.Value).HasMaxLength(CustomIdentifier.MaximumLength);
  }
}
