using MediatR;

namespace Logitar.Identity.Sessions.Queries;

/// <summary>
/// The query raised to retrieve a list of user sessions.
/// </summary>
/// <param name="IsActive">The value filtering user sessions on their activation status.</param>
/// <param name="IsPersistent">The value filtering user sessions on their persistence status.</param>
/// <param name="Realm">The identifier or unique name of the realm to filter by.</param>
/// <param name="UserId">The identifier of the user to filter the sessions by.</param>
/// <param name="Sort">The sort value.</param>
/// <param name="IsDescending">If true, the sort will be inverted.</param>
/// <param name="Skip">The number of user sessions to skip.</param>
/// <param name="Take">The number of user sessions to return.</param>
internal record GetSessionsQuery(bool? IsActive, bool? IsPersistent, string? Realm, Guid? UserId,
  SessionSort? Sort, bool IsDescending, int? Skip, int? Take) : IRequest<PagedList<Session>>;
