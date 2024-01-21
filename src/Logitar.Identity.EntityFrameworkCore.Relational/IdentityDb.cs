using Logitar.Data;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public static class IdentityDb
{
  public static class Actors
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Actors));

    public static readonly ColumnId ActorId = new(nameof(ActorEntity.ActorId), Table);
    public static readonly ColumnId DisplayName = new(nameof(ActorEntity.DisplayName), Table);
    public static readonly ColumnId EmailAddress = new(nameof(ActorEntity.EmailAddress), Table);
    public static readonly ColumnId Id = new(nameof(ActorEntity.Id), Table);
    public static readonly ColumnId IsDeleted = new(nameof(ActorEntity.IsDeleted), Table);
    public static readonly ColumnId PictureUrl = new(nameof(ActorEntity.PictureUrl), Table);
    public static readonly ColumnId Type = new(nameof(ActorEntity.Type), Table);
  }

  public static class ApiKeyRoles
  {
    public static readonly TableId Table = new(nameof(IdentityContext.ApiKeyRoles));

    public static readonly ColumnId ApiKeyId = new(nameof(ApiKeyRoleEntity.ApiKeyId), Table);
    public static readonly ColumnId RoleId = new(nameof(ApiKeyRoleEntity.RoleId), Table);
  }

  public static class ApiKeys
  {
    public static readonly TableId Table = new(nameof(IdentityContext.ApiKeys));

    public static readonly ColumnId AggregateId = new(nameof(ApiKeyEntity.AggregateId), Table);
    public static readonly ColumnId ApiKeyId = new(nameof(ApiKeyEntity.ApiKeyId), Table);
    public static readonly ColumnId AuthenticatedOn = new(nameof(ApiKeyEntity.AuthenticatedOn), Table);
    public static readonly ColumnId CreatedBy = new(nameof(ApiKeyEntity.CreatedBy), Table);
    public static readonly ColumnId CreatedOn = new(nameof(ApiKeyEntity.CreatedOn), Table);
    public static readonly ColumnId CustomAttributes = new(nameof(ApiKeyEntity.CustomAttributes), Table);
    public static readonly ColumnId Description = new(nameof(ApiKeyEntity.Description), Table);
    public static readonly ColumnId DisplayName = new(nameof(ApiKeyEntity.DisplayName), Table);
    public static readonly ColumnId ExpiresOn = new(nameof(ApiKeyEntity.ExpiresOn), Table);
    public static readonly ColumnId SecretHash = new(nameof(ApiKeyEntity.SecretHash), Table);
    public static readonly ColumnId TenantId = new(nameof(ApiKeyEntity.TenantId), Table);
    public static readonly ColumnId UpdatedBy = new(nameof(ApiKeyEntity.UpdatedBy), Table);
    public static readonly ColumnId UpdatedOn = new(nameof(ApiKeyEntity.UpdatedOn), Table);
    public static readonly ColumnId Version = new(nameof(ApiKeyEntity.Version), Table);
  }

  public static class CustomAttributes
  {
    public static readonly TableId Table = new(nameof(IdentityContext.CustomAttributes));

    public static readonly ColumnId CustomAttributeId = new(nameof(CustomAttributeEntity.CustomAttributeId), Table);
    public static readonly ColumnId EntityId = new(nameof(CustomAttributeEntity.EntityId), Table);
    public static readonly ColumnId EntityType = new(nameof(CustomAttributeEntity.EntityType), Table);
    public static readonly ColumnId Key = new(nameof(CustomAttributeEntity.Key), Table);
    public static readonly ColumnId Value = new(nameof(CustomAttributeEntity.Value), Table);
    public static readonly ColumnId ValueShortened = new(nameof(CustomAttributeEntity.ValueShortened), Table);
  }

  public static class Roles
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Roles));

    public static readonly ColumnId AggregateId = new(nameof(RoleEntity.AggregateId), Table);
    public static readonly ColumnId CreatedBy = new(nameof(RoleEntity.CreatedBy), Table);
    public static readonly ColumnId CreatedOn = new(nameof(RoleEntity.CreatedOn), Table);
    public static readonly ColumnId CustomAttributes = new(nameof(RoleEntity.CustomAttributes), Table);
    public static readonly ColumnId Description = new(nameof(RoleEntity.Description), Table);
    public static readonly ColumnId DisplayName = new(nameof(RoleEntity.DisplayName), Table);
    public static readonly ColumnId TenantId = new(nameof(RoleEntity.TenantId), Table);
    public static readonly ColumnId UniqueName = new(nameof(RoleEntity.UniqueName), Table);
    public static readonly ColumnId UniqueNameNormalized = new(nameof(RoleEntity.UniqueNameNormalized), Table);
    public static readonly ColumnId UpdatedBy = new(nameof(RoleEntity.UpdatedBy), Table);
    public static readonly ColumnId UpdatedOn = new(nameof(RoleEntity.UpdatedOn), Table);
    public static readonly ColumnId RoleId = new(nameof(RoleEntity.RoleId), Table);
    public static readonly ColumnId Version = new(nameof(RoleEntity.Version), Table);
  }

  public static class Sessions
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Sessions));

    public static readonly ColumnId AggregateId = new(nameof(SessionEntity.AggregateId), Table);
    public static readonly ColumnId CreatedBy = new(nameof(SessionEntity.CreatedBy), Table);
    public static readonly ColumnId CreatedOn = new(nameof(SessionEntity.CreatedOn), Table);
    public static readonly ColumnId CustomAttributes = new(nameof(SessionEntity.CustomAttributes), Table);
    public static readonly ColumnId IsActive = new(nameof(SessionEntity.IsActive), Table);
    public static readonly ColumnId IsPersistent = new(nameof(SessionEntity.IsPersistent), Table);
    public static readonly ColumnId SecretHash = new(nameof(SessionEntity.SecretHash), Table);
    public static readonly ColumnId SessionId = new(nameof(SessionEntity.SessionId), Table);
    public static readonly ColumnId SignedOutBy = new(nameof(SessionEntity.SignedOutBy), Table);
    public static readonly ColumnId SignedOutOn = new(nameof(SessionEntity.SignedOutOn), Table);
    public static readonly ColumnId UpdatedBy = new(nameof(SessionEntity.UpdatedBy), Table);
    public static readonly ColumnId UpdatedOn = new(nameof(SessionEntity.UpdatedOn), Table);
    public static readonly ColumnId UserId = new(nameof(SessionEntity.UserId), Table);
    public static readonly ColumnId Version = new(nameof(SessionEntity.Version), Table);
  }

  public static class TokenBlacklist
  {
    public static readonly TableId Table = new(nameof(IdentityContext.TokenBlacklist));

    public static readonly ColumnId BlacklistedTokenId = new(nameof(BlacklistedTokenEntity.BlacklistedTokenId), Table);
    public static readonly ColumnId Expires = new(nameof(BlacklistedTokenEntity.ExpiresOn), Table);
    public static readonly ColumnId TokenId = new(nameof(BlacklistedTokenEntity.TokenId), Table);
  }

  public static class UserIdentifiers
  {
    public static readonly TableId Table = new(nameof(IdentityContext.UserIdentifiers));

    public static readonly ColumnId Key = new(nameof(UserIdentifierEntity.Key), Table);
    public static readonly ColumnId TenantId = new(nameof(UserIdentifierEntity.TenantId), Table);
    public static readonly ColumnId UserId = new(nameof(UserIdentifierEntity.UserId), Table);
    public static readonly ColumnId UserIdentifierId = new(nameof(UserIdentifierEntity.UserIdentifierId), Table);
    public static readonly ColumnId Value = new(nameof(UserIdentifierEntity.Value), Table);
  }

  public static class UserRoles
  {
    public static readonly TableId Table = new(nameof(IdentityContext.UserRoles));

    public static readonly ColumnId RoleId = new(nameof(UserRoleEntity.RoleId), Table);
    public static readonly ColumnId UserId = new(nameof(UserRoleEntity.UserId), Table);
  }

  public static class Users
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Users));

    public static readonly ColumnId AddressCountry = new(nameof(UserEntity.AddressCountry), Table);
    public static readonly ColumnId AddressFormatted = new(nameof(UserEntity.AddressFormatted), Table);
    public static readonly ColumnId AddressLocality = new(nameof(UserEntity.AddressLocality), Table);
    public static readonly ColumnId AddressPostalCode = new(nameof(UserEntity.AddressPostalCode), Table);
    public static readonly ColumnId AddressRegion = new(nameof(UserEntity.AddressRegion), Table);
    public static readonly ColumnId AddressStreet = new(nameof(UserEntity.AddressStreet), Table);
    public static readonly ColumnId AddressVerifiedBy = new(nameof(UserEntity.AddressVerifiedBy), Table);
    public static readonly ColumnId AddressVerifiedOn = new(nameof(UserEntity.AddressVerifiedOn), Table);
    public static readonly ColumnId AggregateId = new(nameof(UserEntity.AggregateId), Table);
    public static readonly ColumnId AuthenticatedOn = new(nameof(UserEntity.AuthenticatedOn), Table);
    public static readonly ColumnId Birthdate = new(nameof(UserEntity.Birthdate), Table);
    public static readonly ColumnId CreatedBy = new(nameof(UserEntity.CreatedBy), Table);
    public static readonly ColumnId CreatedOn = new(nameof(UserEntity.CreatedOn), Table);
    public static readonly ColumnId CustomAttributes = new(nameof(UserEntity.CustomAttributes), Table);
    public static readonly ColumnId DisabledBy = new(nameof(UserEntity.DisabledBy), Table);
    public static readonly ColumnId DisabledOn = new(nameof(UserEntity.DisabledOn), Table);
    public static readonly ColumnId EmailAddress = new(nameof(UserEntity.EmailAddress), Table);
    public static readonly ColumnId EmailAddressNormalized = new(nameof(UserEntity.EmailAddressNormalized), Table);
    public static readonly ColumnId EmailVerifiedBy = new(nameof(UserEntity.EmailVerifiedBy), Table);
    public static readonly ColumnId EmailVerifiedOn = new(nameof(UserEntity.EmailVerifiedOn), Table);
    public static readonly ColumnId FirstName = new(nameof(UserEntity.FirstName), Table);
    public static readonly ColumnId FullName = new(nameof(UserEntity.FullName), Table);
    public static readonly ColumnId Gender = new(nameof(UserEntity.Gender), Table);
    public static readonly ColumnId HasPassword = new(nameof(UserEntity.HasPassword), Table);
    public static readonly ColumnId IsAddressVerified = new(nameof(UserEntity.IsAddressVerified), Table);
    public static readonly ColumnId IsConfirmed = new(nameof(UserEntity.IsConfirmed), Table);
    public static readonly ColumnId IsDisabled = new(nameof(UserEntity.IsDisabled), Table);
    public static readonly ColumnId IsEmailVerified = new(nameof(UserEntity.IsEmailVerified), Table);
    public static readonly ColumnId IsPhoneVerified = new(nameof(UserEntity.IsPhoneVerified), Table);
    public static readonly ColumnId LastName = new(nameof(UserEntity.LastName), Table);
    public static readonly ColumnId Locale = new(nameof(UserEntity.Locale), Table);
    public static readonly ColumnId MiddleName = new(nameof(UserEntity.MiddleName), Table);
    public static readonly ColumnId Nickname = new(nameof(UserEntity.Nickname), Table);
    public static readonly ColumnId PasswordChangedBy = new(nameof(UserEntity.PasswordChangedBy), Table);
    public static readonly ColumnId PasswordChangedOn = new(nameof(UserEntity.PasswordChangedOn), Table);
    public static readonly ColumnId PasswordHash = new(nameof(UserEntity.PasswordHash), Table);
    public static readonly ColumnId PhoneCountryCode = new(nameof(UserEntity.PhoneCountryCode), Table);
    public static readonly ColumnId PhoneE164Formatted = new(nameof(UserEntity.PhoneE164Formatted), Table);
    public static readonly ColumnId PhoneExtension = new(nameof(UserEntity.PhoneExtension), Table);
    public static readonly ColumnId PhoneNumber = new(nameof(UserEntity.PhoneNumber), Table);
    public static readonly ColumnId PhoneVerifiedBy = new(nameof(UserEntity.PhoneVerifiedBy), Table);
    public static readonly ColumnId PhoneVerifiedOn = new(nameof(UserEntity.PhoneVerifiedOn), Table);
    public static readonly ColumnId Picture = new(nameof(UserEntity.Picture), Table);
    public static readonly ColumnId Profile = new(nameof(UserEntity.Profile), Table);
    public static readonly ColumnId TenantId = new(nameof(UserEntity.TenantId), Table);
    public static readonly ColumnId TimeZone = new(nameof(UserEntity.TimeZone), Table);
    public static readonly ColumnId UniqueName = new(nameof(UserEntity.UniqueName), Table);
    public static readonly ColumnId UniqueNameNormalized = new(nameof(UserEntity.UniqueNameNormalized), Table);
    public static readonly ColumnId UpdatedBy = new(nameof(UserEntity.UpdatedBy), Table);
    public static readonly ColumnId UpdatedOn = new(nameof(UserEntity.UpdatedOn), Table);
    public static readonly ColumnId UserId = new(nameof(UserEntity.UserId), Table);
    public static readonly ColumnId Version = new(nameof(UserEntity.Version), Table);
    public static readonly ColumnId Website = new(nameof(UserEntity.Website), Table);
  }
}
