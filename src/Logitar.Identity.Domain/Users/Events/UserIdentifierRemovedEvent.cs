using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

public record UserIdentifierRemovedEvent : DomainEvent
{
  public string Key { get; }

  public UserIdentifierRemovedEvent(ActorId actorId, string key)
  {
    ActorId = actorId;
    Key = key;
  }
}
