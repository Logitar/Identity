using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

public record UserEmailChangedEvent : DomainEvent
{
  public EmailUnit? Email { get; }

  public UserEmailChangedEvent(ActorId actorId, EmailUnit? email)
  {
    ActorId = actorId;
    Email = email;
  }
}
