using MediatR;

namespace Logitar.Identity.Sessions.Queries;

/// <summary>
/// The handler for <see cref="GetSessionsQuery"/> queries.
/// </summary>
internal class GetSessionsQueryHandler : IRequestHandler<GetSessionsQuery, PagedList<Session>>
{
  /// <summary>
  /// The session querier.
  /// </summary>
  private readonly ISessionQuerier _sessionQuerier;

  /// <summary>
  /// Initializes a new instance of the <see cref="GetSessionsQueryHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="sessionQuerier">The session querier.</param>
  public GetSessionsQueryHandler(ISessionQuerier sessionQuerier)
  {
    _sessionQuerier = sessionQuerier;
  }

  /// <summary>
  /// Handles the specified query instance.
  /// </summary>
  /// <param name="request">The query to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The retrieved user session or null.</returns>
  /// <exception cref="TooManyResultsException">More than one users have been found.</exception>
  public async Task<PagedList<Session>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
  {
    return await _sessionQuerier.GetAsync(request.IsActive, request.IsPersistent, request.Realm, request.UserId,
      request.Sort, request.IsDescending, request.Skip, request.Take, cancellationToken);
  }
}
