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

  /// <summary>
  /// Retrieves an user session by its <see cref="Guid"/>.
  /// </summary>
  /// <param name="id">The aggregate identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user session or null if not found.</returns>
  public async Task<Session?> GetAsync(Guid id, CancellationToken cancellationToken)
  {
    return await GetAsync(new AggregateId(id), cancellationToken);
  }

  /// <summary>
  /// Retrieves a list of user sessions by their aggregate identifier.
  /// </summary>
  /// <param name="ids">The list of aggregate identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of user sessions, or empty if none.</returns>
  public async Task<IEnumerable<Session>> GetAsync(IEnumerable<AggregateId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<string> aggregateIds = ids.Select(id => id.Value).Distinct();

    IEnumerable<SessionEntity> sessions = await _sessions.AsNoTracking()
      .Where(x => aggregateIds.Contains(x.AggregateId))
      .ToArrayAsync(cancellationToken);

    return _mapper.Map<IEnumerable<Session>>(sessions);
  }

  /// <summary>
  /// Retrieves a list of user sessions using the specified filters, sorting and paging arguments.
  /// </summary>
  /// <param name="isActive">The value filtering user sessions on their activation status.</param>
  /// <param name="isPersistent">The value filtering user sessions on their persistence status.</param>
  /// <param name="realm">The identifier or unique name of the realm to filter by.</param>
  /// <param name="userId">The identifier of the user to filter the sessions by.</param>
  /// <param name="sort">The sort value.</param>
  /// <param name="isDescending">If true, the sort will be inverted.</param>
  /// <param name="skip">The number of user sessions to skip.</param>
  /// <param name="take">The number of user sessions to return.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of user sessions, or empty if none found.</returns>
  public async Task<PagedList<Session>> GetAsync(bool? isActive, bool? isPersistent, string? realm, Guid? userId,
    SessionSort? sort, bool isDescending, int? skip, int? take, CancellationToken cancellationToken)
  {
    IQueryable<SessionEntity> query = _sessions.AsNoTracking()
      .Include(x => x.User).ThenInclude(x => x!.Realm);

    if (isActive.HasValue)
    {
      query = query.Where(x => x.IsActive == isActive.Value);
    }
    if (isPersistent.HasValue)
    {
      query = query.Where(x => x.IsPersistent == isPersistent.Value);
    }
    if (realm != null)
    {
      string aggregateId = (Guid.TryParse(realm, out Guid realmId)
        ? new AggregateId(realmId)
        : new AggregateId(realm)).Value;

      query = query.Where(x => x.User!.Realm!.AggregateId == aggregateId || x.User!.Realm.UniqueNameNormalized == realm.ToUpper());
    }
    if (userId.HasValue)
    {
      string aggregateId = new AggregateId(userId.Value).Value;

      query = query.Where(x => x.User!.AggregateId == aggregateId);
    }

    long total = await query.LongCountAsync(cancellationToken);

    if (sort.HasValue)
    {
      switch (sort.Value)
      {
        case SessionSort.SignedOutOn:
          query = isDescending ? query.OrderByDescending(x => x.SignedOutOn) : query.OrderBy(x => x.SignedOutOn);
          break;
        case SessionSort.UpdatedOn:
          query = isDescending ? query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn) : query.OrderBy(x => x.UpdatedOn ?? x.CreatedOn);
          break;
      }
    }

    query = query.ApplyPaging(skip, take);

    SessionEntity[] sessions = await query.ToArrayAsync(cancellationToken);

    return new PagedList<Session>(_mapper.Map<IEnumerable<Session>>(sessions), total);
  }
}
