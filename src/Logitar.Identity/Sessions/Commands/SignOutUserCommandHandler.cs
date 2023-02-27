using Logitar.EventSourcing;
using Logitar.Identity.Users;
using MediatR;

namespace Logitar.Identity.Sessions.Commands;

/// <summary>
/// The handler for <see cref="SignOutUserCommand"/> commands.
/// </summary>
internal class SignOutUserCommandHandler : IRequestHandler<SignOutUserCommand, IEnumerable<Session>>
{
  /// <summary>
  /// The current actor.
  /// </summary>
  private readonly ICurrentActor _currentActor;
  /// <summary>
  /// The event store.
  /// </summary>
  private readonly IEventStore _eventStore;
  /// <summary>
  /// The session querier.
  /// </summary>
  private readonly ISessionQuerier _sessionQuerier;
  /// <summary>
  /// The session repository.
  /// </summary>
  private readonly ISessionRepository _sessionRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="SignOutUserCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  /// <param name="sessionQuerier">The session querier.</param>
  /// <param name="sessionRepository">The session repository.</param>
  public SignOutUserCommandHandler(ICurrentActor currentActor,
    IEventStore eventStore,
    ISessionQuerier sessionQuerier,
    ISessionRepository sessionRepository)
  {
    _currentActor = currentActor;
    _eventStore = eventStore;
    _sessionQuerier = sessionQuerier;
    _sessionRepository = sessionRepository;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="request">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The signed-out user session.</returns>
  /// <exception cref="AggregateNotFoundException{UserAggregate}">The specified user could not be found.</exception>
  public async Task<IEnumerable<Session>> Handle(SignOutUserCommand request, CancellationToken cancellationToken)
  {
    AggregateId userId = new(request.Id);
    UserAggregate user = await _eventStore.LoadAsync<UserAggregate>(userId, cancellationToken)
      ?? throw new AggregateNotFoundException<UserAggregate>(userId);

    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadActiveAsync(user, cancellationToken);
    List<AggregateId> sessionIds = new(capacity: sessions.Count());
    foreach (SessionAggregate session in sessions)
    {
      session.SignOut(_currentActor.Id);
      sessionIds.Add(session.Id);
    }

    await _eventStore.SaveAsync(sessions, cancellationToken);

    return await _sessionQuerier.GetAsync(sessionIds, cancellationToken);
  }
}
