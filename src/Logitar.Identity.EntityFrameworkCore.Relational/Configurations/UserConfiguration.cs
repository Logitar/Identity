﻿using Logitar.EventSourcing;
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
    builder.HasIndex(x => x.EmailAddress);
    builder.HasIndex(x => new { x.TenantId, x.EmailAddressNormalized });
    builder.HasIndex(x => x.EmailVerifiedBy);
    builder.HasIndex(x => x.EmailVerifiedOn);
    builder.HasIndex(x => x.IsEmailVerified);
    builder.HasIndex(x => x.IsConfirmed);
    builder.HasIndex(x => x.FullName);

    builder.Property(x => x.TenantId).HasMaxLength(TenantId.MaximumLength);
    builder.Property(x => x.UniqueName).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.EmailAddress).HasMaxLength(EmailUnit.AddressMaximumLength);
    builder.Property(x => x.EmailAddressNormalized).HasMaxLength(EmailUnit.AddressMaximumLength);
    builder.Property(x => x.EmailVerifiedBy).HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.DisabledBy).HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.FullName).HasMaxLength(byte.MaxValue * 3 + 2); // TODO(fpion): PersonNameUnit + Documentation
  }
}
