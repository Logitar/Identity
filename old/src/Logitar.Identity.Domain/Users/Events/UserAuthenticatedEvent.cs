using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an user is authenticated.
/// </summary>
public class UserAuthenticatedEvent : DomainEvent, INotification;
