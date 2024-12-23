using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a user is authenticated.
/// </summary>
public record UserAuthenticated : DomainEvent, INotification;
