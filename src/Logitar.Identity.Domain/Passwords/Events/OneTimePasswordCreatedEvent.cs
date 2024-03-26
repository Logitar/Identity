using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Passwords.Events;

/// <summary>
/// The event raised when a new One-Time Password (OTP) is created.
/// </summary>
public record OneTimePasswordCreatedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets the tenant identifier of the One-Time Password (OTP).
  /// </summary>
  public TenantId? TenantId { get; }

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
  /// Initializes a new instance of the <see cref="OneTimePasswordCreatedEvent"/> class.
  /// </summary>
  /// <param name="expiresOn">The expiration date and time of the One-Time Password (OTP).</param>
  /// <param name="maximumAttempts">The maximum number of attempts of the One-Time Password (OTP).</param>
  /// <param name="password">The encoded value of the One-Time Password (OTP).</param>
  /// <param name="tenantId">The tenant identifier of the One-Time Password (OTP).</param>
  public OneTimePasswordCreatedEvent(DateTime? expiresOn, int? maximumAttempts, Password password, TenantId? tenantId)
  {
    ExpiresOn = expiresOn;
    MaximumAttempts = maximumAttempts;
    Password = password;
    TenantId = tenantId;
  }
}
