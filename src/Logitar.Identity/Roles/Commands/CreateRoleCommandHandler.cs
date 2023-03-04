using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using MediatR;

namespace Logitar.Identity.Roles.Commands;

/// <summary>
/// The handler for <see cref="CreateRoleCommand"/> commands.
/// </summary>
internal class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Role>
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
  /// The realm repository.
  /// </summary>
  private readonly IRealmRepository _realmRepository;
  /// <summary>
  /// The role querier.
  /// </summary>
  private readonly IRoleQuerier _roleQuerier;
  /// <summary>
  /// The role repository.
  /// </summary>
  private readonly IRoleRepository _roleRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="CreateRoleCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  /// <param name="realmRepository">The realm repository.</param>
  /// <param name="roleQuerier">The role querier.</param>
  /// <param name="roleRepository">The role repository.</param>
  public CreateRoleCommandHandler(ICurrentActor currentActor,
    IEventStore eventStore,
    IRealmRepository realmRepository,
    IRoleQuerier roleQuerier,
    IRoleRepository roleRepository)
  {
    _currentActor = currentActor;
    _eventStore = eventStore;
    _realmRepository = realmRepository;
    _roleQuerier = roleQuerier;
    _roleRepository = roleRepository;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="command">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The created role.</returns>
  /// <exception cref="AggregateNotFoundException{RealmAggregate}">The specified realm could not be found.</exception>
  /// <exception cref="UniqueNameAlreadyUsedException">The specified unique name is already used.</exception>
  /// <exception cref="InvalidOperationException">The role output could not be found.</exception>
  public async Task<Role> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
  {
    CreateRoleInput input = command.Input;

    RealmAggregate realm = await _realmRepository.LoadAsync(input.Realm, cancellationToken)
      ?? throw new AggregateNotFoundException<RealmAggregate>(new AggregateId(input.Realm), nameof(input.Realm));

    if (await _roleRepository.LoadAsync(realm, input.UniqueName, cancellationToken) != null)
    {
      throw new UniqueNameAlreadyUsedException(input.UniqueName, nameof(input.UniqueName));
    }

    Dictionary<string, string>? customAttributes = input.CustomAttributes?.ToDictionary();

    RoleAggregate role = new(_currentActor.Id, realm, input.UniqueName, input.DisplayName,
      input.Description, customAttributes);

    await _eventStore.SaveAsync(role, cancellationToken);

    return await _roleQuerier.GetAsync(role.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The role output (Id={role.Id}) could not be found.");
  }
}
