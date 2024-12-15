using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Passwords.Events;

/// <summary>
/// The event raised when an existing One-Time Password (OTP) is modified.
/// </summary>
public class OneTimePasswordUpdatedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the custom attribute modifications of the One-Time Password (OTP).
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; init; } = [];

  /// <summary>
  /// Gets a value indicating whether or not the One-Time Password (OTP) is being modified.
  /// </summary>
  [JsonIgnore]
  public bool HasChanges => CustomAttributes.Count > 0;
}
