using Logitar.EventSourcing;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Users.Events;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents a user in the identity system. A user generally represents the account of a person.
/// It contains personal information about that person as well as authentication information that could be used to authenticate that person.
/// </summary>
public class User : AggregateRoot
{
  /// <summary>
  /// The updated event.
  /// </summary>
  private UserUpdated _updated = new();

  /// <summary>
  /// Gets the identifier of the user.
  /// </summary>
  public new UserId Id => new(base.Id);
  /// <summary>
  /// Gets the tenant identifier of the user.
  /// </summary>
  public TenantId? TenantId => Id.TenantId;
  /// <summary>
  /// Gets the entity identifier of the user. This identifier is unique within the tenant.
  /// </summary>
  public EntityId EntityId => Id.EntityId;

  /// <summary>
  /// The unique name of the user.
  /// </summary>
  private UniqueName? _uniqueName = null;
  /// <summary>
  /// Gets the unique name of the user.
  /// </summary>
  /// <exception cref="InvalidOperationException">The unique name has not been initialized yet.</exception>
  public UniqueName UniqueName => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");

  /// <summary>
  /// The password of the user.
  /// </summary>
  private Password? _password = null;
  /// <summary>
  /// Gets a value indicating whether or not the user has a password.
  /// </summary>
  public bool HasPassword => _password != null;

  /// <summary>
  /// Gets or sets a value indicating whether or not the user is disabled.
  /// </summary>
  public bool IsDisabled { get; private set; }

  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public Address? Address { get; private set; }
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public Email? Email { get; private set; }
  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public Phone? Phone { get; private set; }
  /// <summary>
  /// Gets a value indicating whether or not the user is confirmed.
  /// A confirmed user has at least one verified contact information.
  /// </summary>
  public bool IsConfirmed => Address?.IsVerified == true || Email?.IsVerified == true || Phone?.IsVerified == true;

  /// <summary>
  /// The first name of the user.
  /// </summary>
  private PersonName? _firstName = null;
  /// <summary>
  /// Gets or sets the first name of the user.
  /// </summary>
  public PersonName? FirstName
  {
    get => _firstName;
    set
    {
      if (_firstName != value)
      {
        _firstName = value;
        FullName = PersonName.BuildFullName(_firstName, _middleName, _lastName);

        _updated.FirstName = new Change<PersonName>(value);
        _updated.FullName = new Change<string>(FullName);
      }
    }
  }
  /// <summary>
  /// The middle name of the user.
  /// </summary>
  private PersonName? _middleName = null;
  /// <summary>
  /// Gets or sets the middle name of the user.
  /// </summary>
  public PersonName? MiddleName
  {
    get => _middleName;
    set
    {
      if (_middleName != value)
      {
        _middleName = value;
        FullName = PersonName.BuildFullName(_firstName, _middleName, _lastName);

        _updated.MiddleName = new Change<PersonName>(value);
        _updated.FullName = new Change<string>(FullName);
      }
    }
  }
  /// <summary>
  /// The last name of the user.
  /// </summary>
  private PersonName? _lastName = null;
  /// <summary>
  /// Gets or sets the last name of the user.
  /// </summary>
  public PersonName? LastName
  {
    get => _lastName;
    set
    {
      if (_lastName != value)
      {
        _lastName = value;
        FullName = PersonName.BuildFullName(_firstName, _middleName, _lastName);

        _updated.LastName = new Change<PersonName>(value);
        _updated.FullName = new Change<string>(FullName);
      }
    }
  }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public string? FullName { get; private set; }
  /// <summary>
  /// The nickname of the user
  /// </summary>
  private PersonName? _nickname = null;
  /// <summary>
  /// Gets or sets the nickname of the user.
  /// </summary>
  public PersonName? Nickname
  {
    get => _nickname;
    set
    {
      if (_nickname != value)
      {
        _nickname = value;
        _updated.Nickname = new Change<PersonName>(value);
      }
    }
  }

  /// <summary>
  /// The birthdate of the user.
  /// </summary>
  private DateTime? _birthdate = null;
  /// <summary>
  /// Gets or sets the birthdate of the user.
  /// </summary>
  /// <exception cref="ArgumentOutOfRangeException">The birthdate was not in the past.</exception>
  public DateTime? Birthdate
  {
    get => _birthdate;
    set
    {
      if (value.HasValue && value.Value.AsUniversalTime() >= DateTime.UtcNow)
      {
        throw new ArgumentOutOfRangeException(nameof(Birthdate), "The birthdate must be set in the past.");
      }

      if (_birthdate != value)
      {
        _birthdate = value;
        _updated.Birthdate = new Change<DateTime?>(value);
      }
    }
  }
  /// <summary>
  /// The gender of the user.
  /// </summary>
  private Gender? _gender = null;
  /// <summary>
  /// Gets or sets the gender of the user.
  /// </summary>
  public Gender? Gender
  {
    get => _gender;
    set
    {
      if (_gender != value)
      {
        _gender = value;
        _updated.Gender = new Change<Gender>(value);
      }
    }
  }
  /// <summary>
  /// The locale of the user.
  /// </summary>
  private Locale? _locale = null;
  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  public Locale? Locale
  {
    get => _locale;
    set
    {
      if (_locale != value)
      {
        _locale = value;
        _updated.Locale = new Change<Locale>(value);
      }
    }
  }
  /// <summary>
  /// The time zone of the user
  /// </summary>
  private TimeZone? _timeZone = null;
  /// <summary>
  /// Gets or sets the time zone of the user.
  /// </summary>
  public TimeZone? TimeZone
  {
    get => _timeZone;
    set
    {
      if (_timeZone != value)
      {
        _timeZone = value;
        _updated.TimeZone = new Change<TimeZone>(value);
      }
    }
  }

  /// <summary>
  /// The URL to the picture of the user.
  /// </summary>
  private Url? _picture = null;
  /// <summary>
  /// Gets or sets the URL to the picture of the user.
  /// </summary>
  public Url? Picture
  {
    get => _picture;
    set
    {
      if (_picture != value)
      {
        _picture = value;
        _updated.Picture = new Change<Url>(value);
      }
    }
  }
  /// <summary>
  /// The URL to the profile page of the user.
  /// </summary>
  private Url? _profile = null;
  /// <summary>
  /// Gets or sets the URL to the profile page of the user.
  /// </summary>
  public Url? Profile
  {
    get => _profile;
    set
    {
      if (_profile != value)
      {
        _profile = value;
        _updated.Profile = new Change<Url>(value);
      }
    }
  }
  /// <summary>
  /// The URL to the website of the user.
  /// </summary>
  private Url? _website = null;
  /// <summary>
  /// Gets or sets the URL to the website of the user.
  /// </summary>
  public Url? Website
  {
    get => _website;
    set
    {
      if (_website != value)
      {
        _website = value;
        _updated.Website = new Change<Url>(value);
      }
    }
  }

  /// <summary>
  /// Gets of sets the date and time of the latest authentication of this user.
  /// </summary>
  public DateTime? AuthenticatedOn { get; private set; }

  /// <summary>
  /// The custom attributes of the user.
  /// </summary>
  private readonly Dictionary<Identifier, string> _customAttributes = [];
  /// <summary>
  /// Gets the custom attributes of the user.
  /// </summary>
  public IReadOnlyDictionary<Identifier, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// The custom identifiers of the user.
  /// </summary>
  private readonly Dictionary<Identifier, CustomIdentifier> _customIdentifiers = [];
  /// <summary>
  /// Gets the custom identifiers of the user.
  /// </summary>
  public IReadOnlyDictionary<Identifier, CustomIdentifier> CustomIdentifiers => _customIdentifiers.AsReadOnly();

  private readonly HashSet<RoleId> _roles = [];
  /// <summary>
  /// Gets the roles of the user.
  /// </summary>
  public IReadOnlyCollection<RoleId> Roles => _roles.ToList().AsReadOnly();

  /// <summary>
  /// Initializes a new instance of the <see cref="User"/> class.
  /// DO NOT use this constructor to create a new user. It is only intended to be used by the event sourcing.
  /// </summary>
  public User() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="User"/> class.
  /// DO use this constructor to create a new user.
  /// </summary>
  /// <param name="uniqueName">The unique name of the user.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="id">The identifier of the user.</param>
  public User(UniqueName uniqueName, ActorId? actorId = null, UserId? id = null) : base((id ?? UserId.NewId()).StreamId)
  {
    Raise(new UserCreated(uniqueName), actorId);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserCreated @event)
  {
    _uniqueName = @event.UniqueName;
  }

  /// <summary>
  /// Adds the specified role to the user, if the user does not already have the specified role.
  /// </summary>
  /// <param name="role">The role to be added.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <exception cref="TenantMismatchException">The role and user tenant identifiers do not match.</exception>
  public void AddRole(Role role, ActorId? actorId = null)
  {
    if (role.TenantId != TenantId)
    {
      throw new TenantMismatchException(TenantId, role.TenantId);
    }

    if (!HasRole(role))
    {
      Raise(new UserRoleAdded(role.Id), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserRoleAdded @event)
  {
    _roles.Add(@event.RoleId);
  }

  /// <summary>
  /// Authenticates the user.
  /// </summary>
  /// <param name="password">The current password of the user.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <exception cref="IncorrectUserPasswordException">The password is incorrect.</exception>
  /// <exception cref="UserHasNoPasswordException">The user has no password.</exception>
  /// <exception cref="UserIsDisabledException">The user is disabled.</exception>
  public void Authenticate(string password, ActorId? actorId = null)
  {
    if (IsDisabled)
    {
      throw new UserIsDisabledException(this);
    }
    else if (_password == null)
    {
      throw new UserHasNoPasswordException(this);
    }
    else if (!_password.IsMatch(password))
    {
      throw new IncorrectUserPasswordException(this, password);
    }

    actorId ??= new(Id.Value);
    Raise(new UserAuthenticated(), actorId.Value);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserAuthenticated @event)
  {
    AuthenticatedOn = @event.OccurredOn;
  }

  /// <summary>
  /// Changes the password of the user, validating its current password.
  /// </summary>
  /// <param name="currentPassword">The current password of the user.</param>
  /// <param name="newPassword">The new password of the user.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <exception cref="IncorrectUserPasswordException">The current password is incorrect.</exception>
  /// <exception cref="UserHasNoPasswordException">The user has no password.</exception>
  /// <exception cref="UserIsDisabledException">The user is disabled.</exception>
  public void ChangePassword(string currentPassword, Password newPassword, ActorId? actorId = null)
  {
    if (IsDisabled)
    {
      throw new UserIsDisabledException(this);
    }
    else if (_password == null)
    {
      throw new UserHasNoPasswordException(this);
    }
    else if (!_password.IsMatch(currentPassword))
    {
      throw new IncorrectUserPasswordException(this, currentPassword);
    }

    actorId ??= new(Id.Value);
    Raise(new UserPasswordChanged(newPassword), actorId.Value);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserPasswordChanged @event)
  {
    _password = @event.Password;
  }

  /// <summary>
  /// Deletes the user, if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId? actorId = null)
  {
    if (!IsDeleted)
    {
      Raise(new UserDeleted(), actorId);
    }
  }

  /// <summary>
  /// Disables the user, if it is enabled.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Disable(ActorId? actorId = null)
  {
    if (!IsDisabled)
    {
      Raise(new UserDisabled(), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Handle(UserDisabled _)
  {
    IsDisabled = true;
  }

  /// <summary>
  /// Enables the user, if it is disabled.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Enable(ActorId? actorId = null)
  {
    if (IsDisabled)
    {
      Raise(new UserEnabled(), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Handle(UserEnabled _)
  {
    IsDisabled = false;
  }

  /// <summary>
  /// Returns a value indicating whether or not the user has the specified role.
  /// </summary>
  /// <param name="role">The role to match.</param>
  /// <returns>True if the user has the specified role, or false otherwise.</returns>
  public bool HasRole(Role role) => HasRole(role.Id);
  /// <summary>
  /// Returns a value indicating whether or not the user has the specified role.
  /// </summary>
  /// <param name="roleId">The role identifier to match.</param>
  /// <returns>True if the user has the specified role, or false otherwise.</returns>
  public bool HasRole(RoleId roleId) => _roles.Contains(roleId);

  /// <summary>
  /// Removes the specified custom attribute on the user.
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
  /// Removes the specified custom identifier on the user.
  /// </summary>
  /// <param name="key">The key of the custom identifier.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void RemoveCustomIdentifier(Identifier key, ActorId? actorId = null)
  {
    if (_customIdentifiers.ContainsKey(key))
    {
      Raise(new UserIdentifierRemoved(key), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserIdentifierRemoved @event)
  {
    _customIdentifiers.Remove(@event.Key);
  }

  /// <summary>
  /// Removes the password of the user, if the user has a password.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void RemovePassword(ActorId? actorId = null)
  {
    if (_password != null)
    {
      Raise(new UserPasswordRemoved(), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserPasswordRemoved @event)
  {
    _password = null;
  }

  /// <summary>
  /// Removes the specified role from the user, if the user has the specified role.
  /// </summary>
  /// <param name="role">The role to be removed.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void RemoveRole(Role role, ActorId? actorId = null)
  {
    if (HasRole(role))
    {
      Raise(new UserRoleRemoved(role.Id), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserRoleRemoved @event)
  {
    _roles.Remove(@event.RoleId);
  }

  /// <summary>
  /// Resets the password of the user.
  /// </summary>
  /// <param name="password">The new password of the user.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <exception cref="UserIsDisabledException">The user is disabled.</exception>
  public void ResetPassword(Password password, ActorId? actorId = null)
  {
    if (IsDisabled)
    {
      throw new UserIsDisabledException(this);
    }

    actorId ??= new(Id.Value);
    Raise(new UserPasswordReset(password), actorId.Value);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserPasswordReset @event)
  {
    _password = @event.Password;
  }

  /// <summary>
  /// Sets the postal address of the user.
  /// </summary>
  /// <param name="address">The postal address.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetAddress(Address? address, ActorId? actorId = null)
  {
    if (Address != address)
    {
      Raise(new UserAddressChanged(address), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserAddressChanged @event)
  {
    Address = @event.Address;
  }

  /// <summary>
  /// Sets the specified custom attribute on the user. If the value is null, empty or only white-space, the custom attribute will be removed.
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
  /// Sets the specified custom identifier on the user.
  /// </summary>
  /// <param name="key">The key of the custom identifier.</param>
  /// <param name="value">The value of the custom identifier.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetCustomIdentifier(Identifier key, CustomIdentifier value, ActorId? actorId = null)
  {
    if (!_customIdentifiers.TryGetValue(key, out CustomIdentifier? existingValue) || existingValue != value)
    {
      Raise(new UserIdentifierChanged(key, value), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserIdentifierChanged @event)
  {
    _customIdentifiers[@event.Key] = @event.Value;
  }

  /// <summary>
  /// Sets the email address of the user.
  /// </summary>
  /// <param name="email">The email address.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetEmail(Email? email, ActorId? actorId = null)
  {
    if (Email != email)
    {
      Raise(new UserEmailChanged(email), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserEmailChanged @event)
  {
    Email = @event.Email;
  }

  /// <summary>
  /// Sets the password of the user.
  /// </summary>
  /// <param name="password">The new password.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetPassword(Password password, ActorId? actorId = null)
  {
    Raise(new UserPasswordUpdated(password), actorId);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserPasswordUpdated @event)
  {
    _password = @event.Password;
  }

  /// <summary>
  /// Sets the phone number of the user.
  /// </summary>
  /// <param name="phone">The phone number.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetPhone(Phone? phone, ActorId? actorId = null)
  {
    if (Phone != phone)
    {
      Raise(new UserPhoneChanged(phone), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserPhoneChanged @event)
  {
    Phone = @event.Phone;
  }

  /// <summary>
  /// Sets the unique name of the user.
  /// </summary>
  /// <param name="uniqueName">The unique name.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetUniqueName(UniqueName uniqueName, ActorId? actorId = null)
  {
    if (_uniqueName != uniqueName)
    {
      Raise(new UserUniqueNameChanged(uniqueName), actorId);
    }
  }
  /// <summary>
  /// Handles the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserUniqueNameChanged @event)
  {
    _uniqueName = @event.UniqueName;
  }

  /// <summary>
  /// Signs-in the user without a password check, opening a new session.
  /// </summary>
  /// <param name="secret">The secret of the session.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <param name="entityId">The identifier of the session.</param>
  /// <returns>The newly opened session.</returns>
  /// <exception cref="IncorrectUserPasswordException">The password is incorrect.</exception>
  /// <exception cref="UserHasNoPasswordException">The user has no password.</exception>
  /// <exception cref="UserIsDisabledException">The user is disabled.</exception>
  public Session SignIn(Password? secret = null, ActorId? actorId = null, EntityId? entityId = null)
  {
    return SignIn(password: null, secret, actorId, entityId);
  }
  /// <summary>
  /// Signs-in the user, opening a new session.
  /// </summary>
  /// <param name="password">The password to check.</param>
  /// <param name="secret">The secret of the session.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <param name="entityId">The identifier of the session.</param>
  /// <returns>The newly opened session.</returns>
  /// <exception cref="IncorrectUserPasswordException">The password is incorrect.</exception>
  /// <exception cref="UserHasNoPasswordException">The user has no password.</exception>
  /// <exception cref="UserIsDisabledException">The user is disabled.</exception>
  public Session SignIn(string? password, Password? secret = null, ActorId? actorId = null, EntityId? entityId = null)
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
    SessionId sessionId = entityId.HasValue ? new SessionId(TenantId, entityId.Value) : SessionId.NewId(TenantId);
    Session session = new(this, secret, actorId, sessionId);
    Raise(new UserSignedIn(session.CreatedOn), actorId.Value);

    return session;
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Handle(UserSignedIn @event)
  {
    AuthenticatedOn = @event.OccurredOn;
  }

  /// <summary>
  /// Applies updates on the user.
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
  protected virtual void Handle(UserUpdated @event)
  {
    if (@event.FirstName != null)
    {
      _firstName = @event.FirstName.Value;
    }
    if (@event.MiddleName != null)
    {
      _middleName = @event.MiddleName.Value;
    }
    if (@event.LastName != null)
    {
      _lastName = @event.LastName.Value;
    }
    if (@event.FullName != null)
    {
      FullName = @event.FullName.Value;
    }
    if (@event.Nickname != null)
    {
      _nickname = @event.Nickname.Value;
    }

    if (@event.Birthdate != null)
    {
      _birthdate = @event.Birthdate.Value;
    }
    if (@event.Gender != null)
    {
      _gender = @event.Gender.Value;
    }
    if (@event.Locale != null)
    {
      _locale = @event.Locale.Value;
    }
    if (@event.TimeZone != null)
    {
      _timeZone = @event.TimeZone.Value;
    }

    if (@event.Picture != null)
    {
      _picture = @event.Picture.Value;
    }
    if (@event.Profile != null)
    {
      _profile = @event.Profile.Value;
    }
    if (@event.Website != null)
    {
      _website = @event.Website.Value;
    }

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
  /// Returns a string representation of the user.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{FullName ?? UniqueName.Value} | {base.ToString()}";
}
