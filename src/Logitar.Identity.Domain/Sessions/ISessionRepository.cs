namespace Logitar.Identity.Domain.Sessions;

public interface ISessionRepository
{
  Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<SessionAggregate> sessions, CancellationToken cancellationToken = default);
}
