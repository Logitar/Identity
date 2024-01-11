﻿using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Roles;

public class RoleManager : IRoleManager
{
  private readonly IRoleRepository _roleRepository;
  private readonly IUserRepository _userRepository;

  public RoleManager(IRoleRepository roleRepository, IUserRepository userRepository)
  {
    _roleRepository = roleRepository;
    _userRepository = userRepository;
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

      IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(role, cancellationToken);
      foreach (UserAggregate user in users)
      {
        user.RemoveRole(role, actorId);
      }
      await _userRepository.SaveAsync(users, cancellationToken);
    }

    await _roleRepository.SaveAsync(role, cancellationToken);
  }
}
