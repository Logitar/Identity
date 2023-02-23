using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.ApiKeys.Commands;

/// <summary>
/// The handler for <see cref="DeleteApiKeyCommand"/> commands.
/// </summary>
internal class DeleteApiKeyCommandHandler : IRequestHandler<DeleteApiKeyCommand, ApiKey>
{
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
  /// Initializes a new instance of the <see cref="DeleteApiKeyCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="apiKeyQuerier">The API key querier.</param>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  public DeleteApiKeyCommandHandler(IApiKeyQuerier apiKeyQuerier,
    ICurrentActor currentActor,
    IEventStore eventStore)
  {
    _currentActor = currentActor;
    _eventStore = eventStore;
    _apiKeyQuerier = apiKeyQuerier;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="command">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deleted API key.</returns>
  /// <exception cref="AggregateNotFoundException{ApiKeyAggregate}">The specified API key could not be found.</exception>
  /// <exception cref="InvalidOperationException">The API key output could not be found.</exception>
  public async Task<ApiKey> Handle(DeleteApiKeyCommand command, CancellationToken cancellationToken)
  {
    AggregateId id = new(command.Id);
    ApiKeyAggregate apiKey = await _eventStore.LoadAsync<ApiKeyAggregate>(id, cancellationToken)
      ?? throw new AggregateNotFoundException<ApiKeyAggregate>(id);
    ApiKey output = await _apiKeyQuerier.GetAsync(apiKey.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The API key output (Id={apiKey.Id}) could not be found.");

    apiKey.Delete(_currentActor.Id);

    await _eventStore.SaveAsync(apiKey, cancellationToken);

    return output;
  }
}
