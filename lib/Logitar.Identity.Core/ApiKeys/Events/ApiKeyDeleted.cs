using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when an API key is deleted.
/// </summary>
public record ApiKeyDeleted : DomainEvent, IDeleteEvent, INotification;
