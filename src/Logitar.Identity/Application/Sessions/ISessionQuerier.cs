using Logitar.Identity.Contracts.Sessions;
using Logitar.Identity.Domain.Sessions;

namespace Logitar.Identity.Application.Sessions;

internal interface ISessionQuerier
{
  Task<Session> ReadAsync(SessionAggregate session, CancellationToken cancellationToken = default);
  Task<Session?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
}
