using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserUpdatedEvent : DomainEvent, INotification
{
  public bool HasChanges => false;
}
