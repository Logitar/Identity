using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordChangedEvent : DomainEvent, INotification
{
  public Password Password { get; }

  public UserPasswordChangedEvent(ActorId actorId, Password password)
  {
    ActorId = actorId;
    Password = password;
  }
}
