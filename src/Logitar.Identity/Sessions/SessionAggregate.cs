﻿using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Sessions.Events;
using Logitar.Identity.Sessions.Validators;
using Logitar.Identity.Users;

namespace Logitar.Identity.Sessions;

/// <summary>
/// The domain aggregate representing an user session. Sessions give their user access to a resource
/// system from a device, in a window of time. Users can terminate their own sessions, but sessions
/// can also be revoked at any moment in time. User sessions can be ephemeral (short-lived with no
/// renewal process) or live indefinitely and be renewed endlessly.
/// </summary>
public class SessionAggregate : AggregateRoot
{
  /// <summary>
  /// The custom attributes of the session.
  /// </summary>
  private readonly Dictionary<string, string> _customAttributes = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionAggregate"/> class using the specified aggregate identifier.
  /// </summary>
  /// <param name="id">The aggregate identifier.</param>
  public SessionAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionAggregate"/> class using the specified arguments.
  /// </summary>
  /// <param name="user">The user to whom the session belongs.</param>
  /// <param name="keyHash">The salted and hashed key of the session.</param>
  /// <param name="customAttributes">The custom attributes of the session.</param>
  public SessionAggregate(UserAggregate user, string? keyHash = null,
    Dictionary<string, string>? customAttributes = null) : base()
  {
    SessionCreatedEvent e = new()
    {
      ActorId = user.Id,
      UserId = user.Id,
      KeyHash = keyHash,
      CustomAttributes = customAttributes ?? new()
    };
    new SessionCreatedValidator().ValidateAndThrow(e);

    ApplyChange(e);
  }

  /// <summary>
  /// Gets or sets the identifier of the user to whom the session belongs.
  /// </summary>
  public AggregateId UserId { get; private set; }

  /// <summary>
  /// Gets or sets the salted and hashed key of the session.
  /// </summary>
  public string? KeyHash { get; private set; }
  /// <summary>
  /// Gets a value indicating whether or not the session is persistent. A session is persistent if a
  /// refresh token has been issued.
  /// </summary>
  public bool IsPersistent => KeyHash != null;

  /// <summary>
  /// Gets or sets a value indicating whether or not the session is active. A session is unactive if
  /// it has been signed-out.
  /// </summary>
  public bool IsActive { get; private set; }

  /// <summary>
  /// Gets the custom attributes of the session.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Applies the specified event to the user session.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(SessionCreatedEvent e)
  {
    UserId = e.UserId;

    IsActive = true;

    Apply((SessionSavedEvent)e);
  }

  /// <summary>
  /// Refreshes the user session.
  /// </summary>
  /// <param name="keyHash">The salted and hashed key of the session.</param>
  /// <param name="customAttributes">The custom attributes of the session.</param>
  /// <exception cref="SessionIsNotActiveException">The current session has been signed-out.</exception>
  public void Refresh(string? keyHash, Dictionary<string, string>? customAttributes)
  {
    if (!IsActive)
    {
      throw new SessionIsNotActiveException(this);
    }

    SessionRefreshedEvent e = new()
    {
      ActorId = UserId,
      KeyHash = keyHash,
      CustomAttributes = customAttributes ?? new()
    };
    new SessionRefreshedValidator().ValidateAndThrow(e);

    ApplyChange(e);
  }
  /// <summary>
  /// Applies the specified event to the user session.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(SessionRefreshedEvent e)
  {
    Apply((SessionSavedEvent)e);
  }

  /// <summary>
  /// Signs-out the user session.
  /// </summary>
  /// <param name="actorId">The identifier of the user who is signing-out the session.</param>
  /// <returns>False if the user session is already signed-out, true otherwise.</returns>
  public bool SignOut(AggregateId actorId)
  {
    if (!IsActive)
    {
      return false;
    }

    ApplyChange(new SessionSignedOutEvent
    {
      ActorId = actorId
    });

    return true;
  }
  /// <summary>
  /// Applies the specified event to the user session.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(SessionSignedOutEvent e)
  {
    IsActive = false;
  }

  /// <summary>
  /// Applies the specified event to the user session.
  /// </summary>
  /// <param name="e">The domain event.</param>
  private void Apply(SessionSavedEvent e)
  {
    KeyHash = e.KeyHash;

    _customAttributes.Clear();
    _customAttributes.AddRange(e.CustomAttributes);
  }
}
