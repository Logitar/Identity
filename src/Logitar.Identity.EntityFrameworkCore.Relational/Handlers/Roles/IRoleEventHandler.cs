using Logitar.Identity.Domain.Roles.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Roles;

public interface IRoleEventHandler
{
  Task HandleAsync(RoleCreatedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(RoleDeletedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(RoleUniqueNameChangedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(RoleUpdatedEvent @event, CancellationToken cancellationToken = default);
}
