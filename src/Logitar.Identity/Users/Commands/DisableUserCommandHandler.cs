using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Users.Commands;

/// <summary>
/// The handler for <see cref="DisableUserCommand"/> commands.
/// </summary>
internal class DisableUserCommandHandler : IRequestHandler<DisableUserCommand, User>
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
  /// The user querier.
  /// </summary>
  private readonly IUserQuerier _userQuerier;

  /// <summary>
  /// Initializes a new instance of the <see cref="DisableUserCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  /// <param name="userQuerier">The user querier.</param>
  public DisableUserCommandHandler(ICurrentActor currentActor,
    IEventStore eventStore,
    IUserQuerier userQuerier)
  {
    _currentActor = currentActor;
    _eventStore = eventStore;
    _userQuerier = userQuerier;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="command">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The disabled user.</returns>
  /// <exception cref="AggregateNotFoundException{UserAggregate}">The specified user could not be found.</exception>
  /// <exception cref="InvalidOperationException">The user output could not be found.</exception>
  public async Task<User> Handle(DisableUserCommand command, CancellationToken cancellationToken)
  {
    AggregateId id = new(command.Id);
    UserAggregate user = await _eventStore.LoadAsync<UserAggregate>(id, cancellationToken)
      ?? throw new AggregateNotFoundException<UserAggregate>(id);

    user.Disable(_currentActor.Id);

    await _eventStore.SaveAsync(user, cancellationToken);

    return await _userQuerier.GetAsync(user.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The user output (Id={user.Id}) could not be found.");
  }
}
