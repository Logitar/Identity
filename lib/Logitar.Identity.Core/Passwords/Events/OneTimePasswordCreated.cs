using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Passwords.Events;

/// <summary>
/// The event raised when a new One-Time Password (OTP) is created.
/// </summary>
public record OneTimePasswordCreated : DomainEvent, INotification
{
  /// <summary>
  /// Gets the encoded value of the One-Time Password (OTP).
  /// </summary>
  public Password Password { get; }

  /// <summary>
  /// Gets the expiration date and time of the One-Time Password (OTP).
  /// </summary>
  public DateTime? ExpiresOn { get; }
  /// <summary>
  /// Gets the maximum number of attempts of the One-Time Password (OTP).
  /// </summary>
  public int? MaximumAttempts { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordCreated"/> class.
  /// </summary>
  /// <param name="expiresOn">The expiration date and time of the One-Time Password (OTP).</param>
  /// <param name="maximumAttempts">The maximum number of attempts of the One-Time Password (OTP).</param>
  /// <param name="password">The encoded value of the One-Time Password (OTP).</param>
  public OneTimePasswordCreated(DateTime? expiresOn, int? maximumAttempts, Password password)
  {
    ExpiresOn = expiresOn;
    MaximumAttempts = maximumAttempts;
    Password = password;
  }
}
