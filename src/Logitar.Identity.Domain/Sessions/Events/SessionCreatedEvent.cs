using Logitar.EventSourcing;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionCreatedEvent : DomainEvent, INotification
{
  public UserId UserId { get; }

  public SessionCreatedEvent(ActorId actorId, UserId userId)
  {
    ActorId = actorId;
    UserId = userId;
  }
}
