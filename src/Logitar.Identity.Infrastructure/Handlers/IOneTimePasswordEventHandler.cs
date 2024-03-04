using Logitar.Identity.Domain.Passwords.Events;

namespace Logitar.Identity.Infrastructure.Handlers;

public interface IOneTimePasswordEventHandler
{
  Task HandleAsync(OneTimePasswordCreatedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(OneTimePasswordDeletedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(OneTimePasswordUpdatedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(OneTimePasswordValidationFailedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(OneTimePasswordValidationSucceededEvent @event, CancellationToken cancellationToken = default);
}
