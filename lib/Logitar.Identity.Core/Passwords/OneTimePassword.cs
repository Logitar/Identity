using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Passwords.Events;

namespace Logitar.Identity.Core.Passwords;

/// <summary>
/// Represents a One-Time Password (OTP) in the identity system. These passwords can be used for multiple purposes, such as Multi-Factor Authentication (MFA).
/// Several attempts can be made for a single password. Passwords can expire, and once they have been successfully validated, then cannot be used again.
/// </summary>
public class OneTimePassword : AggregateRoot
{
  /// <summary>
  /// The updated event.
  /// </summary>
  private OneTimePasswordUpdated _updated = new();

  /// <summary>
  /// The encoded value of the One-Time Password (OTP).
  /// </summary>
  private Password? _password = null;

  /// <summary>
  /// Gets the identifier of the One-Time Password (OTP).
  /// </summary>
  public new ApiKeyId Id => new(base.Id);
  /// <summary>
  /// Gets the tenant identifier of the One-Time Password (OTP).
  /// </summary>
  public TenantId? TenantId => Id.TenantId;
  /// <summary>
  /// Gets the entity identifier of the One-Time Password (OTP). This identifier is unique within the tenant.
  /// </summary>
  public EntityId? EntityId => Id.EntityId;

  /// <summary>
  /// Gets or sets the expiration date and time of the One-Time Password (OTP).
  /// </summary>
  public DateTime? ExpiresOn { get; private set; }
  /// <summary>
  /// Gets or sets the maximum number of attempts of the One-Time Password (OTP).
  /// </summary>
  public int? MaximumAttempts { get; private set; }

  /// <summary>
  /// Gets or sets the number of attempts that have been made to validate this One-Time Password (OTP).
  /// </summary>
  public int AttemptCount { get; private set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the validation of this One-Time Password (OTP) has succeeded.
  /// </summary>
  public bool HasValidationSucceeded { get; private set; }

  /// <summary>
  /// The custom attributes of the One-Time Password (OTP).
  /// </summary>
  private readonly Dictionary<Identifier, string> _customAttributes = [];
  /// <summary>
  /// Gets the custom attributes of the One-Time Password (OTP).
  /// </summary>
  public IReadOnlyDictionary<Identifier, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePassword"/> class.
  /// DO NOT use this constructor to create a new One-Time Password (OTP). It is only intended to be used by the event sourcing.
  /// </summary>
  public OneTimePassword() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePassword"/> class.
  /// </summary>
  /// <param name="password">The encoded value of the One-Time Password (OTP).</param>
  /// <param name="expiresOn">The expiration date and time of the One-Time Password (OTP).</param>
  /// <param name="maximumAttempts">The maximum number of attempts of the One-Time Password (OTP).</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="id">The identifier of the One-Time Password (OTP).</param>
  /// <exception cref="ArgumentOutOfRangeException">The expiration date time was not set in the future, or the maximum number of attempts was negative or zero.</exception>
  public OneTimePassword(Password password, DateTime? expiresOn = null, int? maximumAttempts = null, ActorId? actorId = null, OneTimePasswordId? id = null)
    : base((id ?? OneTimePasswordId.NewId()).StreamId)
  {
    if (expiresOn.HasValue && expiresOn.Value.AsUniversalTime() <= DateTime.UtcNow)
    {
      throw new ArgumentOutOfRangeException(nameof(expiresOn), "The expiration date and time must be set in the future.");
    }
    if (maximumAttempts.HasValue && maximumAttempts.Value < 1)
    {
      throw new ArgumentOutOfRangeException(nameof(maximumAttempts), "There should be at least one attempt to validate the One-Time Password (OTP).");
    }

    Raise(new OneTimePasswordCreated(expiresOn, maximumAttempts, password), actorId);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(OneTimePasswordCreated @event)
  {
    _password = @event.Password;

    ExpiresOn = @event.ExpiresOn;
    MaximumAttempts = @event.MaximumAttempts;
  }

  /// <summary>
  /// Deletes the One-Time Password (OTP), if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId? actorId = null)
  {
    if (!IsDeleted)
    {
      Raise(new OneTimePasswordDeleted(), actorId);
    }
  }

  /// <summary>
  /// Returns a value indicating whether or not the One-Time Password (OTP) is expired.
  /// </summary>
  /// <param name="moment">(Optional) The date and time to verify the expiration. Defaults to now.</param>
  /// <returns>True if the One-Time Password (OTP) is expired, or false otherwise.</returns>
  public bool IsExpired(DateTime? moment = null) => ExpiresOn.HasValue && ExpiresOn.Value <= (moment ?? DateTime.Now);

  /// <summary>
  /// Removes the specified custom attribute on the One-Time Password (OTP).
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(Identifier key)
  {
    if (_customAttributes.Remove(key))
    {
      _updated.CustomAttributes[key] = null;
    }
  }

  /// <summary>
  /// Sets the specified custom attribute on the One-Time Password (OTP).
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  public void SetCustomAttribute(Identifier key, string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      RemoveCustomAttribute(key);
    }
    value = value.Trim();

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || existingValue != value)
    {
      _customAttributes[key] = value;
      _updated.CustomAttributes[key] = value;
    }
  }

  /// <summary>
  /// Applies updates on the One-Time Password (OTP).
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Update(ActorId? actorId = null)
  {
    if (_updated.HasChanges)
    {
      Raise(_updated, actorId, DateTime.Now);
      _updated = new();
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(OneTimePasswordUpdated @event)
  {
    foreach (KeyValuePair<Identifier, string?> customAttribute in @event.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        _customAttributes.Remove(customAttribute.Key);
      }
      else
      {
        _customAttributes[customAttribute.Key] = customAttribute.Value;
      }
    }
  }

  /// <summary>
  /// Attempts validating the One-Time Password (OTP).
  /// </summary>
  /// <param name="password">The password to validate.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <exception cref="OneTimePasswordAlreadyUsedException">The One-Time Password (OTP) has already been used.</exception>
  /// <exception cref="OneTimePasswordIsExpiredException">The One-Time Password (OTP) is expired.</exception>
  /// <exception cref="MaximumAttemptsReachedException">The maximum number of attempts of the One-Time Password (OTP) has been reached.</exception>
  /// <exception cref="IncorrectOneTimePasswordPasswordException">The specified password did not match.</exception>
  public void Validate(string password, ActorId? actorId = null)
  {
    if (HasValidationSucceeded)
    {
      throw new OneTimePasswordAlreadyUsedException(this);
    }
    else if (IsExpired())
    {
      throw new OneTimePasswordIsExpiredException(this);
    }
    else if (MaximumAttempts.HasValue && MaximumAttempts.Value <= AttemptCount)
    {
      throw new MaximumAttemptsReachedException(this, AttemptCount);
    }
    else if (_password == null || !_password.IsMatch(password))
    {
      Raise(new OneTimePasswordValidationFailed(), actorId);
      throw new IncorrectOneTimePasswordPasswordException(this, password);
    }

    Raise(new OneTimePasswordValidationSucceeded(), actorId);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Handle(OneTimePasswordValidationFailed _)
  {
    AttemptCount++;
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Handle(OneTimePasswordValidationSucceeded _)
  {
    AttemptCount++;
    HasValidationSucceeded = true;
  }
}
