using Logitar.Identity.Domain.ApiKeys.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.ApiKeys;

public interface IApiKeyEventHandler
{
  Task HandleAsync(ApiKeyAuthenticatedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(ApiKeyCreatedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(ApiKeyDeletedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(ApiKeyRoleAddedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(ApiKeyRoleRemovedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(ApiKeyUpdatedEvent @event, CancellationToken cancellationToken = default);
}
