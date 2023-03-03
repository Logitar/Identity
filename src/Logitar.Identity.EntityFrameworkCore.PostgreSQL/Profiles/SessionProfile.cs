using AutoMapper;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.Sessions;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Profiles;

/// <summary>
/// The profile used to configure mapping of user sessions.
/// </summary>
internal class SessionProfile : Profile
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionProfile"/> class.
  /// </summary>
  public SessionProfile()
  {
    CreateMap<SessionEntity, Session>()
      .IncludeBase<AggregateEntity, Aggregate>()
      .ForMember(x => x.Id, x => x.MapFrom(MappingHelper.ToGuid))
      .ForMember(x => x.SignedOutBy, x => x.MapFrom(y => MappingHelper.GetActor(y.SignedOutById, y.SignedOutBy)))
      .ForMember(x => x.CustomAttributes, x => x.MapFrom(MappingHelper.GetCustomAttributes));
  }
}
