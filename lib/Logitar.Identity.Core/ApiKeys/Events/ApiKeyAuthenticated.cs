using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when an API key is authenticated.
/// </summary>
public record ApiKeyAuthenticated : DomainEvent, INotification;
