using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

public class UserAggregate : AggregateRoot
{
  private readonly Dictionary<string, string> _customIdentifiers = [];
  /// <summary>
  /// Gets the custom identifiers of the user.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomIdentifiers => _customIdentifiers.AsReadOnly();

  /// <summary>
  /// Removes the specified custom identifier on the user.
  /// </summary>
  /// <param name="key">The key of the custom identifier.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void RemoveCustomIdentifier(string key, ActorId actorId = default)
  {
    key = key.Trim();

    if (_customIdentifiers.ContainsKey(key))
    {
      Raise(new UserIdentifierRemovedEvent(key), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserIdentifierRemovedEvent @event)
  {
    _customIdentifiers.Remove(@event.Key);
  }

  private readonly CustomIdentifierValidator _customIdentifierValidator = new();
  /// <summary>
  /// Sets the specified custom identifier on the user.
  /// </summary>
  /// <param name="key">The key of the custom identifier.</param>
  /// <param name="value">The value of the custom identifier.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetCustomIdentifier(string key, string value, ActorId actorId = default)
  {
    key = key.Trim();
    value = value.Trim();
    _customIdentifierValidator.ValidateAndThrow(key, value);

    if (!_customIdentifiers.TryGetValue(key, out string? existingValue) || existingValue != value)
    {
      Raise(new UserIdentifierChangedEvent(key, value), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserIdentifierChangedEvent @event)
  {
    _customIdentifiers[@event.Key] = @event.Value;
  }

  /// <summary>
  /// Signs-in the user without a password check, opening a new session.
  /// </summary>
  /// <param name="secret">The secret of the session.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <param name="sessionId">The identifier of the session.</param>
  /// <returns>The newly opened session.</returns>
  /// <exception cref="UserIsDisabledException">The user is disabled.</exception>
  public SessionAggregate SignIn(Password? secret = null, ActorId? actorId = null, SessionId? sessionId = null)
  {
    return SignIn(password: null, secret, actorId, sessionId);
  }
  /// <summary>
  /// Signs-in the user, opening a new session.
  /// </summary>
  /// <param name="password">The password to check.</param>
  /// <param name="secret">The secret of the session.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <param name="sessionId">The identifier of the session.</param>
  /// <returns>The newly opened session.</returns>
  /// <exception cref="IncorrectUserPasswordException">The password is incorrect.</exception>
  /// <exception cref="UserHasNoPasswordException">The user has no password.</exception>
  /// <exception cref="UserIsDisabledException">The user is disabled.</exception>
  public SessionAggregate SignIn(string? password, Password? secret = null, ActorId? actorId = null, SessionId? sessionId = null)
  {
    if (IsDisabled)
    {
      throw new UserIsDisabledException(this);
    }
    else if (password != null)
    {
      if (_password == null)
      {
        throw new UserHasNoPasswordException(this);
      }
      else if (!_password.IsMatch(password))
      {
        throw new IncorrectUserPasswordException(this, password);
      }
    }

    actorId ??= new(Id.Value);
    SessionAggregate session = new(this, secret, actorId, sessionId);
    Raise(new UserSignedInEvent(session.CreatedOn), actorId.Value);

    return session;
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserSignedInEvent @event)
  {
    AuthenticatedOn = @event.OccurredOn;
  }
}
