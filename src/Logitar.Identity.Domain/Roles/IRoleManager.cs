﻿using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Roles;

/// <summary>
/// Defines methods to manage roles.
/// </summary>
public interface IRoleManager
{
  /// <summary>
  /// Saves the specified user, performing model validation such as unique name unicity.
  /// </summary>
  /// <param name="role">The role to save.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(RoleAggregate role, ActorId actorId = default, CancellationToken cancellationToken = default);
}
