using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Realms.Commands;

/// <summary>
/// The handler for <see cref="DeleteRealmCommand"/> commands.
/// </summary>
internal class DeleteRealmCommandHandler : IRequestHandler<DeleteRealmCommand, Realm>
{
  /// <summary>
  /// The actor context.
  /// </summary>
  private readonly ICurrentActor _currentActor;
  /// <summary>
  /// The event store.
  /// </summary>
  private readonly IEventStore _eventStore;
  /// <summary>
  /// The realm querier.
  /// </summary>
  private readonly IRealmQuerier _realmQuerier;

  /// <summary>
  /// Initializes a new instance of the <see cref="DeleteRealmCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="eventStore">The event store.</param>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="realmQuerier">The realm querier.</param>
  public DeleteRealmCommandHandler(ICurrentActor currentActor,
    IEventStore eventStore,
    IRealmQuerier realmQuerier)
  {
    _currentActor = currentActor;
    _eventStore = eventStore;
    _realmQuerier = realmQuerier;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="command">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deleted realm.</returns>
  /// <exception cref="AggregateNotFoundException{RealmAggregate}">The specified realm could not be found.</exception>
  /// <exception cref="InvalidOperationException">The realm output could not be found.</exception>
  public async Task<Realm> Handle(DeleteRealmCommand command, CancellationToken cancellationToken)
  {
    AggregateId id = new(command.Id);
    RealmAggregate realm = await _eventStore.LoadAsync<RealmAggregate>(id, cancellationToken)
      ?? throw new AggregateNotFoundException<RealmAggregate>(id);
    Realm output = await _realmQuerier.GetAsync(realm.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The realm output (Id={realm.Id}) could not be found.");

    realm.Delete(_currentActor.Id);

    await _eventStore.SaveAsync(realm, cancellationToken);

    return output;
  }
}
