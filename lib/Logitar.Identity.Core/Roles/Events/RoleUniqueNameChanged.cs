using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Roles.Events;

/// <summary>
/// The event raised when the unique name of a role is changed.
/// </summary>
public record RoleUniqueNameChanged : DomainEvent, INotification
{
  /// <summary>
  /// Gets the new unique name of the role.
  /// </summary>
  public UniqueName UniqueName { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleUniqueNameChanged"/> class.
  /// </summary>
  /// <param name="uniqueName">The new unique name of the role.</param>
  public RoleUniqueNameChanged(UniqueName uniqueName)
  {
    UniqueName = uniqueName;
  }
}
