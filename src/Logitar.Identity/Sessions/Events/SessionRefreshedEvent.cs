namespace Logitar.Identity.Sessions.Events;

/// <summary>
/// Represents the event raised when a <see cref="SessionAggregate"/> is refreshed.
/// </summary>
public record SessionRefreshedEvent : SessionSavedEvent;
