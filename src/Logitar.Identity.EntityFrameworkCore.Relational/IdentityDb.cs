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

  public static class Users
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Users));

    public static readonly ColumnId AggregateId = new(nameof(UserEntity.AggregateId), Table);
    public static readonly ColumnId AuthenticatedOn = new(nameof(UserEntity.AuthenticatedOn), Table);
    public static readonly ColumnId Birthdate = new(nameof(UserEntity.Birthdate), Table);
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
    public static readonly ColumnId IsConfirmed = new(nameof(UserEntity.IsConfirmed), Table);
    public static readonly ColumnId IsDisabled = new(nameof(UserEntity.IsDisabled), Table);
    public static readonly ColumnId IsEmailVerified = new(nameof(UserEntity.IsEmailVerified), Table);
    public static readonly ColumnId LastName = new(nameof(UserEntity.LastName), Table);
    public static readonly ColumnId Locale = new(nameof(UserEntity.Locale), Table);
    public static readonly ColumnId MiddleName = new(nameof(UserEntity.MiddleName), Table);
    public static readonly ColumnId Nickname = new(nameof(UserEntity.Nickname), Table);
    public static readonly ColumnId PasswordChangedBy = new(nameof(UserEntity.PasswordChangedBy), Table);
    public static readonly ColumnId PasswordChangedOn = new(nameof(UserEntity.PasswordChangedOn), Table);
    public static readonly ColumnId PasswordHash = new(nameof(UserEntity.PasswordHash), Table);
    public static readonly ColumnId Picture = new(nameof(UserEntity.Picture), Table);
    public static readonly ColumnId Profile = new(nameof(UserEntity.Profile), Table);
    public static readonly ColumnId TenantId = new(nameof(UserEntity.TenantId), Table);
    public static readonly ColumnId TimeZone = new(nameof(UserEntity.TimeZone), Table);
    public static readonly ColumnId UniqueName = new(nameof(UserEntity.UniqueName), Table);
    public static readonly ColumnId UniqueNameNormalized = new(nameof(UserEntity.UniqueNameNormalized), Table);
    public static readonly ColumnId UserId = new(nameof(UserEntity.UserId), Table);
    public static readonly ColumnId Website = new(nameof(UserEntity.Website), Table);
  }
}
