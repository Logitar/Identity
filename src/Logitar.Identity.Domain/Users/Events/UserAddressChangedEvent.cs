using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserAddressChangedEvent : DomainEvent, INotification
{
  public AddressUnit? Address { get; }

  public UserAddressChangedEvent(ActorId actorId, AddressUnit? address)
  {
    ActorId = actorId;
    Address = address;
  }
}
