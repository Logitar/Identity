using AutoMapper;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.Users;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Profiles;

/// <summary>
/// The profile used to configure mapping of users.
/// </summary>
internal class UserProfile : Profile
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserProfile"/> class.
  /// </summary>
  public UserProfile()
  {
    CreateMap<UserEntity, User>()
      .IncludeBase<AggregateEntity, Aggregate>()
      .ForMember(x => x.Id, x => x.MapFrom(MappingHelper.ToGuid))
      .ForMember(x => x.PasswordChangedBy, x => x.MapFrom(y => MappingHelper.GetActor(y.PasswordChangedById, y.PasswordChangedBy)))
      .ForMember(x => x.DisabledBy, x => x.MapFrom(y => MappingHelper.GetActor(y.DisabledById, y.DisabledBy)))
      .ForMember(x => x.Address, x => x.MapFrom(GetAddress))
      .ForMember(x => x.Email, x => x.MapFrom(GetEmail))
      .ForMember(x => x.Phone, x => x.MapFrom(GetPhone))
      .ForMember(x => x.CustomAttributes, x => x.MapFrom(MappingHelper.GetCustomAttributes));
    CreateMap<ExternalIdentifierEntity, ExternalIdentifier>()
      .ForMember(x => x.CreatedBy, x => x.MapFrom(y => MappingHelper.GetActor(y.CreatedById, y.CreatedBy)))
      .ForMember(x => x.UpdatedBy, x => x.MapFrom(y => MappingHelper.GetActor(y.UpdatedById ?? y.CreatedById, y.UpdatedBy ?? y.CreatedBy)))
      .ForMember(x => x.UpdatedOn, x => x.MapFrom(y => y.UpdatedOn ?? y.CreatedOn));
  }

  /// <summary>
  /// Resolves the postal address from the specified user entity.
  /// </summary>
  /// <param name="entity">The user entity.</param>
  /// <param name="user">The user output model.</param>
  /// <returns>The postal address.</returns>
  private static Address? GetAddress(UserEntity entity, User user)
  {
    if (entity.AddressLine1 == null || entity.AddressLocality == null || entity.AddressCountry == null)
    {
      return null;
    }

    return new Address
    {
      Line1 = entity.AddressLine1,
      Line2 = entity.AddressLine2,
      Locality = entity.AddressLocality,
      PostalCode = entity.AddressPostalCode,
      Country = entity.AddressCountry,
      Region = entity.AddressRegion,
      VerifiedBy = MappingHelper.GetActor(entity.AddressVerifiedById, entity.AddressVerifiedBy),
      VerifiedOn = entity.AddressVerifiedOn,
      IsVerified = entity.IsAddressVerified
    };
  }

  /// <summary>
  /// Resolves the email address from the specified user entity.
  /// </summary>
  /// <param name="entity">The user entity.</param>
  /// <param name="user">The user output model.</param>
  /// <returns>The email address.</returns>
  private static Email? GetEmail(UserEntity entity, User user)
  {
    if (entity.EmailAddress == null)
    {
      return null;
    }

    return new Email
    {
      Address = entity.EmailAddress,
      VerifiedBy = MappingHelper.GetActor(entity.EmailVerifiedById, entity.EmailVerifiedBy),
      VerifiedOn = entity.EmailVerifiedOn,
      IsVerified = entity.IsEmailVerified
    };
  }

  /// <summary>
  /// Resolves the phone number from the specified user entity.
  /// </summary>
  /// <param name="entity">The user entity.</param>
  /// <param name="user">The user output model.</param>
  /// <returns>The phone number.</returns>
  private static Phone? GetPhone(UserEntity entity, User user)
  {
    if (entity.PhoneNumber == null)
    {
      return null;
    }

    return new Phone
    {
      CountryCode = entity.PhoneCountryCode,
      Number = entity.PhoneNumber,
      Extension = entity.PhoneExtension,
      PhoneE164Formatted = entity.PhoneE164Formatted,
      VerifiedBy = MappingHelper.GetActor(entity.PhoneVerifiedById, entity.PhoneVerifiedBy),
      VerifiedOn = entity.PhoneVerifiedOn,
      IsVerified = entity.IsPhoneVerified
    };
  }
}
