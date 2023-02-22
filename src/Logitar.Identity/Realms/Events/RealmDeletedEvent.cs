using Logitar.EventSourcing;

namespace Logitar.Identity.Realms.Events;

/// <summary>
/// The event raised when a <see cref="RealmAggregate"/> is deleted.
/// </summary>
internal record RealmDeletedEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RealmDeletedEvent"/> class.
  /// </summary>
  public RealmDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
