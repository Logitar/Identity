using Logitar.EventSourcing;
using Logitar.Identity.ApiKeys;
using Logitar.Identity.Roles;
using Logitar.Identity.Sessions;
using Logitar.Identity.Users;
using MediatR;

namespace Logitar.Identity.Realms.Commands;

/// <summary>
/// The handler for <see cref="DeleteRealmCommand"/> commands.
/// </summary>
internal class DeleteRealmCommandHandler : IRequestHandler<DeleteRealmCommand, Realm>
{
  /// <summary>
  /// The API key repository.
  /// </summary>
  private readonly IApiKeyRepository _apiKeyRepository;
  /// <summary>
  /// The current actor.
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
  /// The role repository.
  /// </summary>
  private readonly IRoleRepository _roleRepository;
  /// <summary>
  /// The session repository.
  /// </summary>
  private readonly ISessionRepository _sessionRepository;
  /// <summary>
  /// The user repository.
  /// </summary>
  private readonly IUserRepository _userRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="DeleteRealmCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="apiKeyRepository">The API key repository.</param>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  /// <param name="realmQuerier">The realm querier.</param>
  /// <param name="roleRepository">The role repository.</param>
  /// <param name="sessionRepository">The session repository.</param>
  /// <param name="userRepository">The user repository.</param>
  public DeleteRealmCommandHandler(IApiKeyRepository apiKeyRepository,
    ICurrentActor currentActor,
    IEventStore eventStore,
    IRealmQuerier realmQuerier,
    IRoleRepository roleRepository,
    ISessionRepository sessionRepository,
    IUserRepository userRepository)
  {
    _apiKeyRepository = apiKeyRepository;
    _currentActor = currentActor;
    _eventStore = eventStore;
    _realmQuerier = realmQuerier;
    _roleRepository = roleRepository;
    _sessionRepository = sessionRepository;
    _userRepository = userRepository;
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

    await DeleteSessionsAsync(realm, cancellationToken);
    await DeleteUsersAsync(realm, cancellationToken);

    await DeleteApiKeysAsync(realm, cancellationToken);

    await DeleteRolesAsync(realm, cancellationToken);

    realm.Delete(_currentActor.Id);

    await _eventStore.SaveAsync(realm, cancellationToken);

    return output;
  }

  /// <summary>
  /// Deletes the API keys in the specified realm.
  /// </summary>
  /// <param name="realm">The realm to delete the API keys into.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  private async Task DeleteApiKeysAsync(RealmAggregate realm, CancellationToken cancellationToken)
  {
    IEnumerable<ApiKeyAggregate> apiKeys = await _apiKeyRepository.LoadAsync(realm, cancellationToken);

    foreach (ApiKeyAggregate apiKey in apiKeys)
    {
      apiKey.Delete(_currentActor.Id);
    }

    await _eventStore.SaveAsync(apiKeys, cancellationToken);
  }

  /// <summary>
  /// Deletes the roles in the specified realm.
  /// </summary>
  /// <param name="realm">The realm to delete the roles into.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  private async Task DeleteRolesAsync(RealmAggregate realm, CancellationToken cancellationToken)
  {
    IEnumerable<RoleAggregate> roles = await _roleRepository.LoadAsync(realm, cancellationToken);

    foreach (RoleAggregate role in roles)
    {
      role.Delete(_currentActor.Id);
    }

    await _eventStore.SaveAsync(roles, cancellationToken);
  }

  /// <summary>
  /// Deletes the sessions in the specified realm.
  /// </summary>
  /// <param name="realm">The realm to delete the sessions into.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  private async Task DeleteSessionsAsync(RealmAggregate realm, CancellationToken cancellationToken)
  {
    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(realm, cancellationToken);

    foreach (SessionAggregate session in sessions)
    {
      session.Delete(_currentActor.Id);
    }

    await _eventStore.SaveAsync(sessions, cancellationToken);
  }

  /// <summary>
  /// Deletes the users in the specified realm.
  /// </summary>
  /// <param name="realm">The realm to delete the users into.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  private async Task DeleteUsersAsync(RealmAggregate realm, CancellationToken cancellationToken)
  {
    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(realm, cancellationToken);

    foreach (UserAggregate user in users)
    {
      user.Delete(_currentActor.Id);
    }

    await _eventStore.SaveAsync(users, cancellationToken);
  }
}
