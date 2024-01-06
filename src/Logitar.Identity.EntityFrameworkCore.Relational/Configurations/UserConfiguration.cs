using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class UserConfiguration : AggregateConfiguration<UserEntity>, IEntityTypeConfiguration<UserEntity>
{
  public override void Configure(EntityTypeBuilder<UserEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(IdentityContext.Users));
    builder.HasKey(x => x.UserId);

    builder.HasIndex(x => x.TenantId);
    builder.HasIndex(x => x.UniqueName);
    builder.HasIndex(x => new { x.TenantId, x.UniqueNameNormalized }).IsUnique();
    builder.HasIndex(x => x.PasswordChangedBy);
    builder.HasIndex(x => x.PasswordChangedOn);
    builder.HasIndex(x => x.HasPassword);
    builder.HasIndex(x => x.EmailAddress);
    builder.HasIndex(x => new { x.TenantId, x.EmailAddressNormalized }).IsUnique();
    builder.HasIndex(x => x.EmailVerifiedBy);
    builder.HasIndex(x => x.EmailVerifiedOn);
    builder.HasIndex(x => x.IsEmailVerified);
    builder.HasIndex(x => x.IsConfirmed);
    builder.HasIndex(x => x.FirstName);
    builder.HasIndex(x => x.MiddleName);
    builder.HasIndex(x => x.LastName);
    builder.HasIndex(x => x.FullName);
    builder.HasIndex(x => x.Nickname);
    builder.HasIndex(x => x.Birthdate);
    builder.HasIndex(x => x.AuthenticatedOn);

    builder.Property(x => x.TenantId).HasMaxLength(AggregateId.MaximumLength);
    builder.Property(x => x.UniqueName).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.PasswordHash).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.PasswordChangedBy).HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.EmailAddress).HasMaxLength(EmailUnit.MaximumLength);
    builder.Property(x => x.EmailAddressNormalized).HasMaxLength(EmailUnit.MaximumLength);
    builder.Property(x => x.EmailVerifiedBy).HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.FirstName).HasMaxLength(PersonNameUnit.MaximumLength);
    builder.Property(x => x.MiddleName).HasMaxLength(PersonNameUnit.MaximumLength);
    builder.Property(x => x.LastName).HasMaxLength(PersonNameUnit.MaximumLength);
    builder.Property(x => x.FullName).HasMaxLength(PersonNameUnit.MaximumLength * 3 + 2); // NOTE(fpion): enough space to contain the first, middle and last names, separator by a space ' '.
    builder.Property(x => x.Nickname).HasMaxLength(PersonNameUnit.MaximumLength);
  }
}
