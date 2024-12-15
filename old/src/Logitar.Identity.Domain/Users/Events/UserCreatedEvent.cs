using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when a new user is created.
/// </summary>
public class UserCreatedEvent : DomainEvent, INotification
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
  /// <param name="tenantId">The tenant identifier of the user.</param>
  /// <param name="uniqueName">The unique name of the user.</param>
  public UserCreatedEvent(TenantId? tenantId, UniqueNameUnit uniqueName)
  {
    TenantId = tenantId;
    UniqueName = uniqueName;
  }
}
