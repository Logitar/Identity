using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Roles.Events;

/// <summary>
/// The event raised when a new role is created.
/// </summary>
public record RoleCreatedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the tenant identifier of the role.
  /// </summary>
  public TenantId? TenantId { get; }

  /// <summary>
  /// Gets the unique name of the role.
  /// </summary>
  public UniqueNameUnit UniqueName { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleCreatedEvent"/> class.
  /// </summary>
  /// <param name="tenantId">The tenant identifier of the role.</param>
  /// <param name="uniqueName">The unique name of the role.</param>
  public RoleCreatedEvent(TenantId? tenantId, UniqueNameUnit uniqueName)
  {
    TenantId = tenantId;
    UniqueName = uniqueName;
  }
}
