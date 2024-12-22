using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public sealed class ActorConfiguration : IEntityTypeConfiguration<ActorEntity>
{
  public void Configure(EntityTypeBuilder<ActorEntity> builder)
  {
    builder.ToTable(IdentityDb.Actors.Table.Table ?? string.Empty, IdentityDb.Actors.Table.Schema);
    builder.HasKey(x => x.ActorId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.Type);
    builder.HasIndex(x => x.IsDeleted);
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.EmailAddress);

    builder.Property(x => x.Id).IsRequired().HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.Type).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.EmailAddress).HasMaxLength(Email.MaximumLength);
    builder.Property(x => x.PictureUrl).HasMaxLength(Url.MaximumLength);
  }
}
