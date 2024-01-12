using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

/// <summary>
/// The event raised when a new API key is created.
/// </summary>
public record ApiKeyCreatedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the secret of the API key.
  /// </summary>
  public Password Secret { get; }

  /// <summary>
  /// Gets the tenant identifier of the API key.
  /// </summary>
  public TenantId? TenantId { get; }

  /// <summary>
  /// Gets the display name of the API key.
  /// </summary>
  public DisplayNameUnit DisplayName { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyCreatedEvent"/> class.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="displayName">The display name of the API key.</param>
  /// <param name="secret">The secret of the API key.</param>
  /// <param name="tenantId">The tenant identifier of the API key.</param>
  public ApiKeyCreatedEvent(ActorId actorId, DisplayNameUnit displayName, Password secret, TenantId? tenantId)
  {
    ActorId = actorId;
    DisplayName = displayName;
    Secret = secret;
    TenantId = tenantId;
  }
}
