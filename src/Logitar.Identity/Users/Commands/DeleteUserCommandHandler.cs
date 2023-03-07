using Logitar.EventSourcing;
using Logitar.Identity.Sessions;
using MediatR;

namespace Logitar.Identity.Users.Commands;

/// <summary>
/// The handler for <see cref="DeleteUserCommand"/> commands.
/// </summary>
internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, User>
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
  /// The session repository.
  /// </summary>
  private readonly ISessionRepository _sessionRepository;
  /// <summary>
  /// The user querier.
  /// </summary>
  private readonly IUserQuerier _userQuerier;

  /// <summary>
  /// Initializes a new instance of the <see cref="DeleteUserCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  /// <param name="sessionRepository">The session repository.</param>
  /// <param name="userQuerier">The user querier.</param>
  public DeleteUserCommandHandler(ICurrentActor currentActor,
    IEventStore eventStore,
    ISessionRepository sessionRepository,
    IUserQuerier userQuerier)
  {
    _currentActor = currentActor;
    _eventStore = eventStore;
    _sessionRepository = sessionRepository;
    _userQuerier = userQuerier;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="command">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deleted user.</returns>
  /// <exception cref="AggregateNotFoundException{UserAggregate}">The specified user could not be found.</exception>
  /// <exception cref="InvalidOperationException">The user output could not be found.</exception>
  public async Task<User> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
  {
    AggregateId id = new(command.Id);
    UserAggregate user = await _eventStore.LoadAsync<UserAggregate>(id, cancellationToken)
      ?? throw new AggregateNotFoundException<UserAggregate>(id);
    User output = await _userQuerier.GetAsync(user.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The user output (Id={user.Id}) could not be found.");

    await DeleteSessionsAsync(user, cancellationToken);

    user.Delete(_currentActor.Id);

    await _eventStore.SaveAsync(user, cancellationToken);

    return output;
  }

  /// <summary>
  /// Deletes the sessions of the specified user.
  /// </summary>
  /// <param name="user">The user to delete its sessions.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  private async Task DeleteSessionsAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(user, cancellationToken);

    foreach (SessionAggregate session in sessions)
    {
      session.Delete(_currentActor.Id);
    }

    await _eventStore.SaveAsync(sessions, cancellationToken);
  }
}
