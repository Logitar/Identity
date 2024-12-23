using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a user is enabled.
/// </summary>
public record UserEnabled : DomainEvent, INotification;
