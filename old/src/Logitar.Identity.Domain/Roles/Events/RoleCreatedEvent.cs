using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Roles.Events;

public class RoleCreatedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the tenant identifier of the role.
  /// </summary>
  public TenantId? TenantId { get; }
}
