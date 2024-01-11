﻿using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Domain.ApiKeys.Events;

/// <summary>
/// The event raised when a role is removed from an API key.
/// </summary>
public record ApiKeyRoleRemovedEvent : DomainEvent
{
  /// <summary>
  /// Gets the role identifier.
  /// </summary>
  public RoleId RoleId { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyRoleRemovedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="roleId">The role identifier.</param>
  public ApiKeyRoleRemovedEvent(ActorId actorId, RoleId roleId)
  {
    ActorId = actorId;
    RoleId = roleId;
  }
}