using Logitar.Identity.Domain.Shared;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class CustomAttributeConfiguration : IEntityTypeConfiguration<CustomAttributeEntity>
{
  public void Configure(EntityTypeBuilder<CustomAttributeEntity> builder)
  {
    builder.ToTable(nameof(IdentityContext.CustomAttributes));
    builder.HasKey(x => x.CustomAttributeId);

    builder.HasIndex(x => new { x.EntityType, x.EntityId });
    builder.HasIndex(x => new { x.EntityType, x.EntityId, x.Key }).IsUnique();
    builder.HasIndex(x => x.Key);
    builder.HasIndex(x => x.ValueShortened);

    builder.Property(x => x.EntityType).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Key).HasMaxLength(IdentifierValidator.MaximumLength);
    builder.Property(x => x.ValueShortened).HasMaxLength(CustomAttributeEntity.ValueShortenedLength);
  }
}
