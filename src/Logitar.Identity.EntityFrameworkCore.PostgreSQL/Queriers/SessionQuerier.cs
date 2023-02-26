using AutoMapper;
using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Queriers;

/// <summary>
/// Implements methods used to query user session read models.
/// </summary>
internal class SessionQuerier : ISessionQuerier
{
  /// <summary>
  /// The mapper instance.
  /// </summary>
  private readonly IMapper _mapper;
  /// <summary>
  /// The data set of user sessions.
  /// </summary>
  private readonly DbSet<SessionEntity> _sessions;

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionQuerier"/> class.
  /// </summary>
  /// <param name="context">The identity context.</param>
  /// <param name="mapper">The mapper instance.</param>
  public SessionQuerier(IdentityContext context, IMapper mapper)
  {
    _mapper = mapper;
    _sessions = context.Sessions;
  }

  /// <summary>
  /// Retrieves an user session by its aggregate identifier.
  /// </summary>
  /// <param name="id">The aggregate identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user session or null if not found.</returns>
  public async Task<Session?> GetAsync(AggregateId id, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _sessions.AsNoTracking()
      .Include(x => x.User).ThenInclude(x => x!.ExternalIdentifiers)
      .Include(x => x.User).ThenInclude(x => x!.Realm)
      .Include(x => x.User).ThenInclude(x => x!.Roles)
      .SingleOrDefaultAsync(x => x.AggregateId == id.Value, cancellationToken);

    return _mapper.Map<Session>(session);
  }
}
