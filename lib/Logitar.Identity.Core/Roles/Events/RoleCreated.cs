using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Roles.Events;

/// <summary>
/// The event raised when a new role is created.
/// </summary>
public record RoleCreated : DomainEvent, INotification
{
  /// <summary>
  /// Gets the unique name.
  /// </summary>
  public UniqueName UniqueName { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleCreated"/> class.
  /// </summary>
  /// <param name="uniqueName">The unique name.</param>
  public RoleCreated(UniqueName uniqueName)
  {
    UniqueName = uniqueName;
  }
}
