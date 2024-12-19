using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the unique name of an user is changed.
/// </summary>
public record UserUniqueNameChanged : DomainEvent, INotification
{
  /// <summary>
  /// Gets the unique name of the user.
  /// </summary>
  public UniqueName UniqueName { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserUniqueNameChanged"/> class.
  /// </summary>
  /// <param name="uniqueName">The unique name of the user.</param>
  public UserUniqueNameChanged(UniqueName uniqueName)
  {
    UniqueName = uniqueName;
  }
}
