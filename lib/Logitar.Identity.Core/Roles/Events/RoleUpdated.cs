using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Roles.Events;

/// <summary>
/// The event raised when a role is updated.
/// </summary>
public record RoleUpdated : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the new unique name of the role.
  /// </summary>
  public UniqueName? UniqueName { get; set; }
  /// <summary>
  /// Gets or sets the new display name of the role.
  /// </summary>
  public Change<DisplayName>? DisplayName { get; set; }
  /// <summary>
  /// Gets or sets the new description of the role.
  /// </summary>
  public Change<Description>? Description { get; set; }

  // TODO(fpion): CustomAttributes

  /// <summary>
  /// Gets a value indicating whether or not the role has been updated.
  /// </summary>
  //[JsonIgnore] // TODO(fpion): implement
  public bool HasChanges => UniqueName != null || DisplayName != null || Description != null;
}
