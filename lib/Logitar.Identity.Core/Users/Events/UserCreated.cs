using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when a new user is created.
/// </summary>
public record UserCreated : DomainEvent, INotification
{
  /// <summary>
  /// Gets the unique name of the user.
  /// </summary>
  public UniqueName UniqueName { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserCreated"/> class.
  /// </summary>
  /// <param name="uniqueName">The unique name of the user.</param>
  public UserCreated(UniqueName uniqueName)
  {
    UniqueName = uniqueName;
  }
}
