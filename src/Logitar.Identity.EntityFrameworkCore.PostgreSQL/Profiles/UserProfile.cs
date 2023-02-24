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
      .ForMember(x => x.CustomAttributes, x => x.MapFrom(MappingHelper.GetCustomAttributes));
    CreateMap<ExternalIdentifierEntity, ExternalIdentifier>()
      .ForMember(x => x.CreatedBy, x => x.MapFrom(y => MappingHelper.GetActor(y.CreatedById, y.CreatedBy)))
      .ForMember(x => x.UpdatedBy, x => x.MapFrom(y => MappingHelper.GetActor(y.UpdatedById ?? y.CreatedById, y.UpdatedBy ?? y.CreatedBy)))
      .ForMember(x => x.UpdatedOn, x => x.MapFrom(y => y.UpdatedOn ?? y.CreatedOn));
  }
}
