using Logitar.EventSourcing;

namespace Logitar.Identity.Users.Events;

/// <summary>
/// Defines properties of the events setting phone numbers.
/// </summary>
public interface ISetPhone
{
  /// <summary>
  /// Gets the identifier of the actor setting the phone number.
  /// </summary>
  public AggregateId ActorId { get; }
  /// <summary>
  /// Gets or sets the date and time when the phone number was set.
  /// </summary>
  public DateTime OccurredOn { get; }

  /// <summary>
  /// Gets the phone number of the user.
  /// </summary>
  public ReadOnlyPhone? Phone { get; }
  /// <summary>
  /// Gets the phone number verification action performed by the event.
  /// </summary>
  public VerificationAction PhoneVerification { get; }
}
