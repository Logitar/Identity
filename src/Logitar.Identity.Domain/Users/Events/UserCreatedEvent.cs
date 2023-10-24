using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when a new user is created.
/// </summary>
public record UserCreatedEvent : DomainEvent
{
  /// <summary>
  /// Gets the tenant identifier of the user.
  /// </summary>
  public TenantId? TenantId { get; }

  /// <summary>
  /// Gets the unique name of the user.
  /// </summary>
  public UniqueNameUnit UniqueName { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserCreatedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="uniqueName">The unique name of the user.</param>
  /// <param name="tenantId">The tenant identifier of the user.</param>
  public UserCreatedEvent(ActorId actorId, UniqueNameUnit uniqueName, TenantId? tenantId = null)
  {
    ActorId = actorId;
    TenantId = tenantId;
    UniqueName = uniqueName;
  }
}
