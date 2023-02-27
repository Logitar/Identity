using Logitar.EventSourcing;

namespace Logitar.Identity.Sessions;

/// <summary>
/// Exposes methods used to query user session read models.
/// </summary>
public interface ISessionQuerier
{
  /// <summary>
  /// Retrieves an user session by its aggregate identifier.
  /// </summary>
  /// <param name="id">The aggregate identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user session or null if not found.</returns>
  Task<Session?> GetAsync(AggregateId id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Retrieves an user session by its <see cref="Guid"/>.
  /// </summary>
  /// <param name="id">The aggregate identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The user session or null if not found.</returns>
  Task<Session?> GetAsync(Guid id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Retrieves a list of user sessions by their aggregate identifier.
  /// </summary>
  /// <param name="ids">The list of aggregate identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The list of user sessions, or empty if none.</returns>
  Task<IEnumerable<Session>> GetAsync(IEnumerable<AggregateId> ids, CancellationToken cancellationToken = default);
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
  Task<PagedList<Session>> GetAsync(bool? isActive = null, bool? isPersistent = null, string? realm = null, Guid? userId = null,
    SessionSort? sort = null, bool isDescending = false, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
}
