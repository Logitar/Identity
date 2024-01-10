using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPhoneChangedEvent : DomainEvent, INotification
{
  public PhoneUnit? Phone { get; }

  public UserPhoneChangedEvent(ActorId actorId, PhoneUnit? phone)
  {
    ActorId = actorId;
    Phone = phone;
  }
}
