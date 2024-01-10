using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain.Users.Events;

public abstract record UserPasswordEvent : DomainEvent
{
  public Password Password { get; }

  protected UserPasswordEvent(ActorId actorId, Password password)
  {
    ActorId = actorId;
    Password = password;
  }
}
