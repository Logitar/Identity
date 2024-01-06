using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserEmailChangedEvent : DomainEvent, INotification
{
  public EmailUnit? Email { get; }

  public UserEmailChangedEvent(ActorId actorId, EmailUnit? email)
  {
    ActorId = actorId;
    Email = email;
  }
}
