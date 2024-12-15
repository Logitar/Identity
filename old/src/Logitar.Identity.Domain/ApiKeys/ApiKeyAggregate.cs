using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Shared.Validators;

namespace Logitar.Identity.Domain.ApiKeys;

public class ApiKeyAggregate : AggregateRoot
{
  private Password? _secret = null;

  /// <summary>
  /// Gets the tenant identifier of the API key.
  /// </summary>
  public TenantId? TenantId { get; private set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyAggregate"/> class.
  /// DO NOT use this constructor to create a new API key. It is only intended to be used by the event sourcing.
  /// </summary>
  public ApiKeyAggregate() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyAggregate"/> class.
  /// DO use this constructor to create a new API key.
  /// </summary>
  /// <param name="displayName">The unique name of the API key.</param>
  /// <param name="secret">The secret of the API key.</param>
  /// <param name="tenantId">The tenant identifier of the API key.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="id">The identifier of the API key.</param>
  public ApiKeyAggregate(DisplayNameUnit displayName, Password secret, TenantId? tenantId = null, ActorId actorId = default, ApiKeyId? id = null)
    : base((id ?? ApiKeyId.NewId()).AggregateId)
  {
    Raise(new ApiKeyCreatedEvent(displayName, secret, tenantId), actorId);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(ApiKeyCreatedEvent @event)
  {
    _secret = @event.Secret;

    TenantId = @event.TenantId;

    _displayName = @event.DisplayName;
  }
}
