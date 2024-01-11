using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserIdentifierChangedEvent : DomainEvent, INotification
{
  public string Key { get; }
  public string Value { get; }

  public UserIdentifierChangedEvent(ActorId actorId, string key, string value)
  {
    ActorId = actorId;
    Key = key;
    Value = value;
  }
}
