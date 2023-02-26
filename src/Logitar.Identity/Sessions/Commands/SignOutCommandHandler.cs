using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Sessions.Commands;

/// <summary>
/// The handler for <see cref="SignOutCommand"/> commands.
/// </summary>
internal class SignOutCommandHandler : IRequestHandler<SignOutCommand, Session>
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
  /// Initializes a new instance of the <see cref="SignOutCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  /// <param name="sessionQuerier">The session querier.</param>
  public SignOutCommandHandler(ICurrentActor currentActor,
    IEventStore eventStore,
    ISessionQuerier sessionQuerier)
  {
    _currentActor = currentActor;
    _eventStore = eventStore;
    _sessionQuerier = sessionQuerier;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="request">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The signed-out user session.</returns>
  /// <exception cref="AggregateNotFoundException{SessionAggregate}">The specified user session could not be found.</exception>
  /// <exception cref="InvalidOperationException">The user session output could not be found.</exception>
  public async Task<Session> Handle(SignOutCommand request, CancellationToken cancellationToken)
  {
    AggregateId sessionId = new(request.Id);
    SessionAggregate session = await _eventStore.LoadAsync<SessionAggregate>(sessionId, cancellationToken)
      ?? throw new AggregateNotFoundException<SessionAggregate>(sessionId);

    if (session.SignOut(_currentActor.Id))
    {
      await _eventStore.SaveAsync(session, cancellationToken);
    }

    return await _sessionQuerier.GetAsync(session.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The user session output (Id={session.Id}) could not be found.");
  }
}
