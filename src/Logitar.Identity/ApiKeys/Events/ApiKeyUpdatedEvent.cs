using MediatR;

namespace Logitar.Identity.ApiKeys.Events;

/// <summary>
/// Represents the event raised when an <see cref="ApiKeyAggregate"/> is updated.
/// </summary>
public record ApiKeyUpdatedEvent : ApiKeySavedEvent, INotification;
