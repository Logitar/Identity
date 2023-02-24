using Logitar.Identity.Actors;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Configurations;

/// <summary>
/// The model configuration used to configure classes inheriting the <see cref="ExternalIdentifierEntity"/> class.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class ExternalIdentifierConfiguration : IEntityTypeConfiguration<ExternalIdentifierEntity>
{
  /// <summary>
  /// Configures the database model using the specified type builder.
  /// </summary>
  /// <param name="builder">The type builder.</param>
  public void Configure(EntityTypeBuilder<ExternalIdentifierEntity> builder)
  {
    Actor system = new();
    string json = new ActorEntity(system).Serialize();

    builder.HasKey(x => x.ExternalIdentifierId);

    builder.HasOne(x => x.User).WithMany(x => x.ExternalIdentifiers).OnDelete(DeleteBehavior.Cascade);

    builder.HasIndex(x => x.CreatedById);
    builder.HasIndex(x => x.UpdatedById);
    builder.HasIndex(x => new { x.RealmId, x.Key, x.Value }).IsUnique();

    builder.Property(x => x.Key).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Value).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.CreatedById).HasMaxLength(byte.MaxValue).HasDefaultValue(system.Id);
    builder.Property(x => x.CreatedBy).HasColumnType("jsonb").HasDefaultValue(json);
    builder.Property(x => x.CreatedOn).HasDefaultValueSql("now()");
    builder.Property(x => x.UpdatedById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UpdatedBy).HasColumnType("jsonb");
  }
}
