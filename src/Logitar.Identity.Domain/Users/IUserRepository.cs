using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

public interface IUserRepository
{
  Task<UserAggregate?> LoadAsync(TenantId? tenantId, UniqueNameUnit uniqueName, CancellationToken cancellationToken = default);

  Task SaveAsync(UserAggregate user, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken = default);
}
