using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordChangedEvent : DomainEvent
{
  public Password Password { get; }

  public UserPasswordChangedEvent(ActorId actorId, Password password)
  {
    ActorId = actorId;
    Password = password;
  }
}
