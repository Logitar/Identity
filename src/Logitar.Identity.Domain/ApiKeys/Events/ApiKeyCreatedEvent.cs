using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys.Events;

public record ApiKeyCreatedEvent : DomainEvent
{
  public Password Secret { get; }

  public TenantId? TenantId { get; }

  public DisplayNameUnit DisplayName { get; }

  public ApiKeyCreatedEvent(ActorId actorId, DisplayNameUnit displayName, Password secret, TenantId? tenantId)
  {
    ActorId = actorId;
    DisplayName = displayName;
    Secret = secret;
    TenantId = tenantId;
  }
}
