using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords.Events;
using Logitar.Identity.Domain.Passwords.Validators;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Shared.Validators;

namespace Logitar.Identity.Domain.Passwords;

/// <summary>
/// Represents a One-Time Password (OTP) in the identity system. These passwords can be used for multiple purposes, such as Multi-Factor Authentication (MFA).
/// Several attempts can be made for a single password. Passwords can expire, and once they have been successfully validated, then cannot be used again.
/// </summary>
public class OneTimePasswordAggregate : AggregateRoot
{
  private Password? _password = null;
  private OneTimePasswordUpdatedEvent _updatedEvent = new();

  /// <summary>
  /// Gets the identifier of the One-Time Password (OTP).
  /// </summary>
  public new OneTimePasswordId Id => new(base.Id);

  /// <summary>
  /// Gets the tenant identifier of the One-Time Password (OTP).
  /// </summary>
  public TenantId? TenantId { get; private set; }

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

  private readonly Dictionary<string, string> _customAttributes = [];
  /// <summary>
  /// Gets the custom attributes of the One-Time Password (OTP).
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordAggregate"/> class.
  /// DO NOT use this constructor to create a new One-Time Password (OTP). It is only intended to be used by the event sourcing.
  /// </summary>
  /// <param name="id">The identifier of the One-Time Password (OTP).</param>
  public OneTimePasswordAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordAggregate"/> class.
  /// </summary>
  /// <param name="password">The encoded value of the One-Time Password (OTP).</param>
  /// <param name="tenantId">The tenant identifier of the One-Time Password (OTP).</param>
  /// <param name="expiresOn">The expiration date and time of the One-Time Password (OTP).</param>
  /// <param name="maximumAttempts">The maximum number of attempts of the One-Time Password (OTP).</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="id">The identifier of the One-Time Password (OTP).</param>
  public OneTimePasswordAggregate(Password password, TenantId? tenantId = null, DateTime? expiresOn = null, int? maximumAttempts = null, ActorId actorId = default, OneTimePasswordId? id = null)
    : base((id ?? OneTimePasswordId.NewId()).AggregateId)
  {
    if (expiresOn.HasValue)
    {
      new ExpirationValidator().ValidateAndThrow(expiresOn.Value);
    }
    if (maximumAttempts.HasValue)
    {
      new MaximumAttemptsValidator().ValidateAndThrow(maximumAttempts.Value);
    }

    Raise(new OneTimePasswordCreatedEvent(actorId, expiresOn, maximumAttempts, password, tenantId));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(OneTimePasswordCreatedEvent @event)
  {
    _password = @event.Password;

    TenantId = @event.TenantId;

    ExpiresOn = @event.ExpiresOn;
    MaximumAttempts = @event.MaximumAttempts;
  }

  /// <summary>
  /// Deletes the One-Time Password (OTP), if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new OneTimePasswordDeletedEvent(actorId));
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
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();

    if (_customAttributes.ContainsKey(key))
    {
      _updatedEvent.CustomAttributes[key] = null;
      _customAttributes.Remove(key);
    }
  }

  private readonly CustomAttributeValidator _customAttributeValidator = new();
  /// <summary>
  /// Sets the specified custom attribute on the One-Time Password (OTP).
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    _customAttributeValidator.ValidateAndThrow(key, value);

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || existingValue != value)
    {
      _updatedEvent.CustomAttributes[key] = value;
      _customAttributes[key] = value;
    }
  }

  /// <summary>
  /// Applies updates on the One-Time Password (OTP).
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Update(ActorId actorId = default)
  {
    if (_updatedEvent.HasChanges)
    {
      _updatedEvent.ActorId = actorId;
      Raise(_updatedEvent);
      _updatedEvent = new();
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(OneTimePasswordUpdatedEvent @event)
  {
    foreach (KeyValuePair<string, string?> customAttribute in @event.CustomAttributes)
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
  public void Validate(string password, ActorId actorId = default)
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
      Raise(new OneTimePasswordValidationFailedEvent(actorId));
      throw new IncorrectOneTimePasswordPasswordException(this, password);
    }

    Raise(new OneTimePasswordValidationSucceededEvent(actorId));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Apply(OneTimePasswordValidationFailedEvent _)
  {
    AttemptCount++;
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Apply(OneTimePasswordValidationSucceededEvent _)
  {
    AttemptCount++;
    HasValidationSucceeded = true;
  }
}
