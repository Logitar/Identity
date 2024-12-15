using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

internal static class AssertUsers
{
  public static void AreEqual(UserAggregate? user, UserEntity? entity)
  {
    if (user == null || entity == null)
    {
      Assert.Null(user);
      Assert.Null(entity);
      return;
    }

    Assert.Equal(user.Version, entity.Version);
    Assert.Equal(user.CreatedBy.Value, entity.CreatedBy);
    Assertions.Equal(user.CreatedOn, entity.CreatedOn, TimeSpan.FromSeconds(1));
    Assert.Equal(user.UpdatedBy.Value, entity.UpdatedBy);
    Assertions.Equal(user.UpdatedOn, entity.UpdatedOn, TimeSpan.FromSeconds(1));

    Assert.Equal(user.TenantId?.Value, entity.TenantId);

    Assert.Equal(user.UniqueName.Value, entity.UniqueName);
    Assert.Equal(user.UniqueName.Value.ToUpper(), entity.UniqueNameNormalized);

    if (user.HasPassword)
    {
      Assert.True(entity.HasPassword);
      Assert.NotNull(entity.PasswordChangedBy);
      Assert.NotNull(entity.PasswordChangedOn);
      Assert.NotNull(entity.PasswordHash);
    }
    else
    {
      Assert.False(entity.HasPassword);
      Assert.Null(entity.PasswordChangedBy);
      Assert.Null(entity.PasswordChangedOn);
      Assert.Null(entity.PasswordHash);
    }

    if (user.IsDisabled)
    {
      Assert.NotNull(entity.DisabledBy);
      Assert.NotNull(entity.DisabledOn);
      Assert.True(entity.IsDisabled);
    }
    else
    {
      Assert.Null(entity.DisabledBy);
      Assert.Null(entity.DisabledOn);
      Assert.False(entity.IsDisabled);
    }

    Assert.Equal(user.Address?.Street, entity.AddressStreet);
    Assert.Equal(user.Address?.Locality, entity.AddressLocality);
    Assert.Equal(user.Address?.PostalCode, entity.AddressPostalCode);
    Assert.Equal(user.Address?.Region, entity.AddressRegion);
    Assert.Equal(user.Address?.Country, entity.AddressCountry);
    Assert.Equal(user.Address?.Format(), entity.AddressFormatted);
    if (user.Address?.IsVerified == true)
    {
      Assert.NotNull(entity.AddressVerifiedBy);
      Assert.NotNull(entity.AddressVerifiedOn);
      Assert.True(entity.IsAddressVerified);
    }
    else
    {
      Assert.Null(entity.AddressVerifiedBy);
      Assert.Null(entity.AddressVerifiedOn);
      Assert.False(entity.IsAddressVerified);
    }

    Assert.Equal(user.Email?.Address, entity.EmailAddress);
    Assert.Equal(user.Email?.Address.ToUpper(), entity.EmailAddressNormalized);
    if (user.Email?.IsVerified == true)
    {
      Assert.NotNull(entity.EmailVerifiedBy);
      Assert.NotNull(entity.EmailVerifiedOn);
      Assert.True(entity.IsEmailVerified);
    }
    else
    {
      Assert.Null(entity.EmailVerifiedBy);
      Assert.Null(entity.EmailVerifiedOn);
      Assert.False(entity.IsEmailVerified);
    }

    Assert.Equal(user.Phone?.CountryCode, entity.PhoneCountryCode);
    Assert.Equal(user.Phone?.Number, entity.PhoneNumber);
    Assert.Equal(user.Phone?.Extension, entity.PhoneExtension);
    Assert.Equal(user.Phone?.FormatToE164(), entity.PhoneE164Formatted);
    if (user.Phone?.IsVerified == true)
    {
      Assert.NotNull(entity.PhoneVerifiedBy);
      Assert.NotNull(entity.PhoneVerifiedOn);
      Assert.True(entity.IsPhoneVerified);
    }
    else
    {
      Assert.Null(entity.PhoneVerifiedBy);
      Assert.Null(entity.PhoneVerifiedOn);
      Assert.False(entity.IsPhoneVerified);
    }

    Assert.Equal(user.IsConfirmed, entity.IsConfirmed);

    Assert.Equal(user.FirstName?.Value, entity.FirstName);
    Assert.Equal(user.MiddleName?.Value, entity.MiddleName);
    Assert.Equal(user.LastName?.Value, entity.LastName);
    Assert.Equal(user.FullName, entity.FullName);
    Assert.Equal(user.Nickname?.Value, entity.Nickname);

    Assertions.Equal(user.Birthdate, entity.Birthdate, TimeSpan.FromSeconds(1));
    Assert.Equal(user.Gender?.Value, entity.Gender);
    Assert.Equal(user.Locale?.Code, entity.Locale);
    Assert.Equal(user.TimeZone?.Id, entity.TimeZone);

    Assert.Equal(user.Picture?.Value, entity.Picture);
    Assert.Equal(user.Profile?.Value, entity.Profile);
    Assert.Equal(user.Website?.Value, entity.Website);

    Assertions.Equal(user.AuthenticatedOn, entity.AuthenticatedOn, TimeSpan.FromSeconds(1));

    Assert.Equal(user.CustomAttributes, entity.CustomAttributes);

    foreach (KeyValuePair<string, string> customIdentifier in user.CustomIdentifiers)
    {
      UserIdentifierEntity? identitiferEntity = entity.Identifiers.SingleOrDefault(x => x.Key == customIdentifier.Key);
      Assert.NotNull(identitiferEntity);
      Assert.Equal(entity.UserId, identitiferEntity.UserId);
      Assert.Equal(user.TenantId?.Value, identitiferEntity.TenantId);
      Assert.Equal(customIdentifier.Value, identitiferEntity.Value);
    }

    foreach (RoleId roleId in user.Roles)
    {
      Assert.Contains(entity.Roles, role => role.AggregateId == roleId.Value);
    }
  }

  public static void AreEquivalent(UserEntity? user, ActorEntity? actor)
  {
    if (user == null || actor == null)
    {
      Assert.Null(user);
      Assert.Null(actor);
      return;
    }

    Assert.Equal(user.AggregateId, actor.Id);
    Assert.Equal(ActorType.User, actor.Type);
    Assert.False(actor.IsDeleted);
    Assert.Equal(user.FullName, actor.DisplayName);
    Assert.Equal(user.EmailAddress, actor.EmailAddress);
    Assert.Equal(user.Picture, actor.PictureUrl);
  }
}
