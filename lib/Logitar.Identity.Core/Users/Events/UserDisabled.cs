using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a user is disabled.
/// </summary>
public record UserDisabled : DomainEvent, INotification;
