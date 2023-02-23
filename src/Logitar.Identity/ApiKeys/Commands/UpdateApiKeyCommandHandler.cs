using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using Logitar.Identity.Roles;
using MediatR;

namespace Logitar.Identity.ApiKeys.Commands;

/// <summary>
/// The handler for <see cref="UpdateApiKeyCommand"/> commands.
/// </summary>
internal class UpdateApiKeyCommandHandler : IRequestHandler<UpdateApiKeyCommand, ApiKey>
{
  /// <summary>
  /// The API key helper.
  /// </summary>
  private readonly IApiKeyHelper _apiKeyHelper;
  /// <summary>
  /// The API key querier.
  /// </summary>
  private readonly IApiKeyQuerier _apiKeyQuerier;
  /// <summary>
  /// The current actor.
  /// </summary>
  private readonly ICurrentActor _currentActor;
  /// <summary>
  /// The event store.
  /// </summary>
  private readonly IEventStore _eventStore;

  /// <summary>
  /// Initializes a new instance of the <see cref="UpdateApiKeyCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="apiKeyHelper">The API key helper.</param>
  /// <param name="apiKeyQuerier">The API key querier.</param>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  public UpdateApiKeyCommandHandler(IApiKeyHelper apiKeyHelper,
    IApiKeyQuerier apiKeyQuerier,
    ICurrentActor currentActor,
    IEventStore eventStore)
  {
    _apiKeyHelper = apiKeyHelper;
    _apiKeyQuerier = apiKeyQuerier;
    _currentActor = currentActor;
    _eventStore = eventStore;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="command">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The updated API key.</returns>
  /// <exception cref="AggregateNotFoundException{ApiKeyAggregate}">The specified API key could not be found.</exception>
  /// <exception cref="InvalidOperationException">The API key output could not be found.</exception>
  public async Task<ApiKey> Handle(UpdateApiKeyCommand command, CancellationToken cancellationToken)
  {
    AggregateId id = new(command.Id);
    ApiKeyAggregate apiKey = await _eventStore.LoadAsync<ApiKeyAggregate>(id, cancellationToken)
      ?? throw new AggregateNotFoundException<ApiKeyAggregate>(id);
    RealmAggregate realm = await _eventStore.LoadAsync<RealmAggregate>(apiKey.RealmId, cancellationToken)
      ?? throw new InvalidOperationException($"The realm 'Id={apiKey.RealmId}' could not be found.");

    UpdateApiKeyInput input = command.Input;

    Dictionary<string, string>? customAttributes = input.CustomAttributes?.ToDictionary();
    IEnumerable<RoleAggregate>? roles = await _apiKeyHelper.GetRolesAsync(realm, input, cancellationToken);

    apiKey.Update(_currentActor.Id, input.Title, input.Description, customAttributes, roles);

    await _eventStore.SaveAsync(apiKey, cancellationToken);

    return await _apiKeyQuerier.GetAsync(apiKey.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The API key output (Id={apiKey.Id}) could not be found.");
  }
}
