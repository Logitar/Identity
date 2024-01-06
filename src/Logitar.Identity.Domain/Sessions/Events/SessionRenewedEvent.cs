using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionRenewedEvent : DomainEvent, INotification
{
  public Password Secret { get; }

  public SessionRenewedEvent(ActorId actorId, Password secret)
  {
    ActorId = actorId;
    Secret = secret;
  }
}
