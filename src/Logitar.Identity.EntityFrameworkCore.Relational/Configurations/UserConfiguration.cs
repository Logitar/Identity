using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class UserConfiguration : AggregateConfiguration<UserEntity>, IEntityTypeConfiguration<UserEntity>
{
  public const int AddressFormattedMaximumLength = AddressUnit.MaximumLength * 5 + 4; // NOTE(fpion): enough space to contain the five address components, each separated by one character.
  public const int FullNameMaximumLength = PersonNameUnit.MaximumLength * 3 + 2; // NOTE(fpion): enough space to contain the first, middle and last names, separator by a space ' '.
  public const int PhoneE164FormattedMaximumLength = PhoneUnit.CountryCodeLength + 1 + PhoneUnit.NumberMaximumLength + 7 + PhoneUnit.ExtensionMaximumLength; // NOTE(fpion): enough space to contain the following format '{CountryCode} {Number}, ext. {Extension}'.

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
    builder.HasIndex(x => x.DisabledBy);
    builder.HasIndex(x => x.DisabledOn);
    builder.HasIndex(x => x.IsDisabled);
    builder.HasIndex(x => x.AddressStreet);
    builder.HasIndex(x => x.AddressLocality);
    builder.HasIndex(x => x.AddressPostalCode);
    builder.HasIndex(x => x.AddressRegion);
    builder.HasIndex(x => x.AddressCountry);
    builder.HasIndex(x => x.AddressFormatted);
    builder.HasIndex(x => x.AddressVerifiedBy);
    builder.HasIndex(x => x.AddressVerifiedOn);
    builder.HasIndex(x => x.IsAddressVerified);
    builder.HasIndex(x => x.EmailAddress);
    builder.HasIndex(x => new { x.TenantId, x.EmailAddressNormalized });
    builder.HasIndex(x => x.EmailVerifiedBy);
    builder.HasIndex(x => x.EmailVerifiedOn);
    builder.HasIndex(x => x.IsEmailVerified);
    builder.HasIndex(x => x.PhoneCountryCode);
    builder.HasIndex(x => x.PhoneNumber);
    builder.HasIndex(x => x.PhoneExtension);
    builder.HasIndex(x => x.PhoneE164Formatted);
    builder.HasIndex(x => x.PhoneVerifiedBy);
    builder.HasIndex(x => x.PhoneVerifiedOn);
    builder.HasIndex(x => x.IsPhoneVerified);
    builder.HasIndex(x => x.IsConfirmed);
    builder.HasIndex(x => x.FirstName);
    builder.HasIndex(x => x.MiddleName);
    builder.HasIndex(x => x.LastName);
    builder.HasIndex(x => x.FullName);
    builder.HasIndex(x => x.Nickname);
    builder.HasIndex(x => x.Birthdate);
    builder.HasIndex(x => x.Gender);
    builder.HasIndex(x => x.Locale);
    builder.HasIndex(x => x.TimeZone);
    builder.HasIndex(x => x.AuthenticatedOn);

    builder.Ignore(x => x.CustomAttributes);

    builder.Property(x => x.TenantId).HasMaxLength(AggregateId.MaximumLength);
    builder.Property(x => x.UniqueName).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.PasswordHash).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.PasswordChangedBy).HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.DisabledBy).HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.AddressStreet).HasMaxLength(AddressUnit.MaximumLength);
    builder.Property(x => x.AddressLocality).HasMaxLength(AddressUnit.MaximumLength);
    builder.Property(x => x.AddressPostalCode).HasMaxLength(AddressUnit.MaximumLength);
    builder.Property(x => x.AddressRegion).HasMaxLength(AddressUnit.MaximumLength);
    builder.Property(x => x.AddressCountry).HasMaxLength(AddressUnit.MaximumLength);
    builder.Property(x => x.AddressFormatted).HasMaxLength(AddressFormattedMaximumLength);
    builder.Property(x => x.AddressVerifiedBy).HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.EmailAddress).HasMaxLength(EmailUnit.MaximumLength);
    builder.Property(x => x.EmailAddressNormalized).HasMaxLength(EmailUnit.MaximumLength);
    builder.Property(x => x.EmailVerifiedBy).HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.PhoneCountryCode).HasMaxLength(PhoneUnit.CountryCodeLength);
    builder.Property(x => x.PhoneNumber).HasMaxLength(PhoneUnit.NumberMaximumLength);
    builder.Property(x => x.PhoneExtension).HasMaxLength(PhoneUnit.ExtensionMaximumLength);
    builder.Property(x => x.PhoneE164Formatted).HasMaxLength(PhoneE164FormattedMaximumLength);
    builder.Property(x => x.PhoneVerifiedBy).HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.FirstName).HasMaxLength(PersonNameUnit.MaximumLength);
    builder.Property(x => x.MiddleName).HasMaxLength(PersonNameUnit.MaximumLength);
    builder.Property(x => x.LastName).HasMaxLength(PersonNameUnit.MaximumLength);
    builder.Property(x => x.FullName).HasMaxLength(FullNameMaximumLength);
    builder.Property(x => x.Nickname).HasMaxLength(PersonNameUnit.MaximumLength);
    builder.Property(x => x.Gender).HasMaxLength(GenderUnit.MaximumLength);
    builder.Property(x => x.Locale).HasMaxLength(LocaleUnit.MaximumLength);
    builder.Property(x => x.TimeZone).HasMaxLength(TimeZoneUnit.MaximumLength);
    builder.Property(x => x.Picture).HasMaxLength(UrlUnit.MaximumLength);
    builder.Property(x => x.Profile).HasMaxLength(UrlUnit.MaximumLength);
    builder.Property(x => x.Website).HasMaxLength(UrlUnit.MaximumLength);
    builder.Property(x => x.CustomAttributesSerialized).HasColumnName(nameof(UserEntity.CustomAttributes));

    builder.HasMany(x => x.Roles).WithMany(x => x.Users).UsingEntity<UserRoleEntity>(joinBuilder =>
    {
      joinBuilder.ToTable(nameof(IdentityContext.UserRoles));
      joinBuilder.HasKey(x => new { x.UserId, x.RoleId });
    });
  }
}
