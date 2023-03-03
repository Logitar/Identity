using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Sessions.Queries;

/// <summary>
/// The handler for <see cref="GetSessionQuery"/> queries.
/// </summary>
internal class GetSessionQueryHandler : IRequestHandler<GetSessionQuery, Session?>
{
  /// <summary>
  /// The session querier.
  /// </summary>
  private readonly ISessionQuerier _sessionQuerier;

  /// <summary>
  /// Initializes a new instance of the <see cref="GetSessionQueryHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="sessionQuerier">The session querier.</param>
  public GetSessionQueryHandler(ISessionQuerier sessionQuerier)
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
  public async Task<Session?> Handle(GetSessionQuery request, CancellationToken cancellationToken)
  {
    List<Session> users = new(capacity: 1);

    if (request.Id.HasValue)
    {
      users.AddIfNotNull(await _sessionQuerier.GetAsync(request.Id.Value, cancellationToken));
    }

    if (users.Count > 1)
    {
      throw new TooManyResultsException();
    }

    return users.SingleOrDefault();
  }
}
