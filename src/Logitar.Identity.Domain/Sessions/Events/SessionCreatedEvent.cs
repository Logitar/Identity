using Logitar.EventSourcing;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionCreatedEvent : DomainEvent
{
  public UserId UserId { get; }

  public SessionCreatedEvent(ActorId actorId, UserId userId)
  {
    ActorId = actorId;
    UserId = userId;
  }
}
