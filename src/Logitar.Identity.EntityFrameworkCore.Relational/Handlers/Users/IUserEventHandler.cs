using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public interface IUserEventHandler
{
  Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserDeletedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserSignedInEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserUniqueNameChangedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserUpdatedEvent @event, CancellationToken cancellationToken = default);
}
