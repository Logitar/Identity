using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Roles;

public class RoleManager : IRoleManager
{
  private readonly IRoleRepository _roleRepository;

  public RoleManager(IRoleRepository roleRepository)
  {
    _roleRepository = roleRepository;
  }

  public async Task SaveAsync(RoleAggregate role, ActorId actorId, CancellationToken cancellationToken)
  {
    bool hasBeenDeleted = false;
    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in role.Changes)
    {
      if (change is RoleCreatedEvent || change is RoleUniqueNameChangedEvent)
      {
        hasUniqueNameChanged = true;
      }
      else if (change is RoleDeletedEvent)
      {
        hasBeenDeleted = true;
      }
    }

    if (hasUniqueNameChanged)
    {
      RoleAggregate? other = await _roleRepository.LoadAsync(role.TenantId, role.UniqueName, cancellationToken);
      if (other?.Equals(role) == false)
      {
        throw new UniqueNameAlreadyUsedException<RoleAggregate>(role.TenantId, role.UniqueName);
      }
    }

    if (hasBeenDeleted)
    {
      // TODO(fpion): remove role from API keys
      // TODO(fpion): remove role from users
    }

    await _roleRepository.SaveAsync(role, cancellationToken);
  }
}
