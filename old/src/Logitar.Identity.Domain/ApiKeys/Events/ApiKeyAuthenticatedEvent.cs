using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

/// <summary>
/// The event raised when an API key is authenticated.
/// </summary>
public class ApiKeyAuthenticatedEvent : DomainEvent, INotification;
