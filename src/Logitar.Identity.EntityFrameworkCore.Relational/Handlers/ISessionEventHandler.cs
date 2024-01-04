using Logitar.Identity.Domain.Sessions.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public interface ISessionEventHandler
{
  Task HandleAsync(SessionCreatedEvent @event, CancellationToken cancellationToken = default);
}
