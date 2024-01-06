using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionCreatedEvent : DomainEvent, INotification
{
  public UserId UserId { get; }

  public Password? Secret { get; }

  public SessionCreatedEvent(ActorId actorId, Password? secret, UserId userId)
  {
    ActorId = actorId;
    Secret = secret;
    UserId = userId;
  }
}
