using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public interface IUserEventHandler
{
  Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserDeletedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserDisabledEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserEnabledEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserUniqueNameChangedEvent @event, CancellationToken cancellationToken = default);
}
