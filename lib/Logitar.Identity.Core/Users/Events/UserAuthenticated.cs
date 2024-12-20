using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when an user is authenticated.
/// </summary>
public record UserAuthenticated : DomainEvent, INotification;
