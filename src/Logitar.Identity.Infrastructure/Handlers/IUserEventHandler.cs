using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Infrastructure.Handlers;

public interface IUserEventHandler
{
  Task HandleAsync(UserAddressChangedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserAuthenticatedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserDeletedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserDisabledEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserEnabledEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserEmailChangedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserIdentifierChangedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserIdentifierRemovedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserPasswordEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserPhoneChangedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserRoleAddedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserRoleRemovedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserSignedInEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserUniqueNameChangedEvent @event, CancellationToken cancellationToken = default);
  Task HandleAsync(UserUpdatedEvent @event, CancellationToken cancellationToken = default);
}
