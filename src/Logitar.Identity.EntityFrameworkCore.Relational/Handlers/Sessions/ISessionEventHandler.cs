using Logitar.Identity.Domain.Sessions.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;

public interface ISessionEventHandler
{
  Task HandleAsync(SessionCreatedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(SessionDeletedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(SessionRenewedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(SessionSignedOutEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(SessionUpdatedEvent @event, CancellationToken cancellationToken = default);
}
