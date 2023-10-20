using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an existing user is modified.
/// </summary>
public record UserUpdatedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the first name of the user.
  /// </summary>
  public Modification<PersonNameUnit>? FirstName { get; internal set; }
  /// <summary>
  /// Gets or sets the middle name of the user.
  /// </summary>
  public Modification<PersonNameUnit>? MiddleName { get; internal set; }
  /// <summary>
  /// Gets or sets the last name of the user.
  /// </summary>
  public Modification<PersonNameUnit>? LastName { get; internal set; }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public Modification<string>? FullName { get; internal set; }
  /// <summary>
  /// Gets or sets the nickname of the user.
  /// </summary>
  public Modification<PersonNameUnit>? Nickname { get; internal set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the user.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; } = new();

  /// <summary>
  /// Gets a value indicating whether or not the user is being modified.
  /// </summary>
  public bool HasChanges => FirstName != null || MiddleName != null || LastName != null || FullName != null || Nickname != null || CustomAttributes.Any();
}
