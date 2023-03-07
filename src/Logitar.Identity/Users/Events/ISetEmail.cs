using Logitar.EventSourcing;

namespace Logitar.Identity.Users.Events;

/// <summary>
/// Defines properties of the events setting email addresses.
/// </summary>
public interface ISetEmail
{
  /// <summary>
  /// Gets the identifier of the actor setting the email address.
  /// </summary>
  public AggregateId ActorId { get; }
  /// <summary>
  /// Gets or sets the date and time when the email address was set.
  /// </summary>
  public DateTime OccurredOn { get; }

  /// <summary>
  /// Gets the email address of the user.
  /// </summary>
  public ReadOnlyEmail? Email { get; }
  /// <summary>
  /// Gets the email address verification action performed by the event.
  /// </summary>
  public VerificationAction EmailVerification { get; }
}
