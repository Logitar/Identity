using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Roles.Events;

/// <summary>
/// The event raised when the unique name of a role is changed.
/// </summary>
public record RoleUniqueNameChangedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the unique name of the role.
  /// </summary>
  public UniqueNameUnit UniqueName { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleUniqueNameChangedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="uniqueName">The unique name of the role.</param>
  public RoleUniqueNameChangedEvent(ActorId actorId, UniqueNameUnit uniqueName)
  {
    ActorId = actorId;
    UniqueName = uniqueName;
  }
}
