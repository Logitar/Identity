using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserIdentifierRemovedEvent : DomainEvent, INotification
{
  public string Key { get; }

  public UserIdentifierRemovedEvent(ActorId actorId, string key)
  {
    ActorId = actorId;
    Key = key;
  }
}
