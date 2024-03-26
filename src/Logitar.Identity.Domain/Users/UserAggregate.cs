using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.Domain.Users.Validators;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents an user in the identity system. An user generally represents the account of a person.
/// It contains personal information about that person as well as authentication information that could be used to authenticate that person.
/// </summary>
public class UserAggregate : AggregateRoot
{
  private UserUpdatedEvent _updatedEvent = new();

  /// <summary>
  /// Gets the identifier of the user.
  /// </summary>
  public new UserId Id => new(base.Id);

  /// <summary>
  /// Gets the tenant identifier of the user.
  /// </summary>
  public TenantId? TenantId { get; private set; }

  /// <summary>
  /// The unique name of the user.
  /// </summary>
  private UniqueNameUnit? _uniqueName = null;
  /// <summary>
  /// Gets the unique name of the user.
  /// </summary>
  /// <exception cref="InvalidOperationException">The unique name has not been initialized yet.</exception>
  public UniqueNameUnit UniqueName => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");

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
  public AddressUnit? Address { get; private set; }
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public EmailUnit? Email { get; private set; }
  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public PhoneUnit? Phone { get; private set; }
  /// <summary>
  /// Gets a value indicating whether or not the user is confirmed.
  /// A confirmed user has at least one verified contact information.
  /// </summary>
  public bool IsConfirmed => Address?.IsVerified == true || Email?.IsVerified == true || Phone?.IsVerified == true;

  private PersonNameUnit? _firstName = null;
  /// <summary>
  /// Gets or sets the first name of the user.
  /// </summary>
  public PersonNameUnit? FirstName
  {
    get => _firstName;
    set
    {
      if (value != _firstName)
      {
        _firstName = value;
        FullName = PersonHelper.BuildFullName(_firstName, _middleName, _lastName);

        _updatedEvent.FirstName = new Modification<PersonNameUnit>(value);
        _updatedEvent.FullName = new Modification<string>(FullName);
      }
    }
  }
  private PersonNameUnit? _middleName = null;
  /// <summary>
  /// Gets or sets the middle name of the user.
  /// </summary>
  public PersonNameUnit? MiddleName
  {
    get => _middleName;
    set
    {
      if (value != _middleName)
      {
        _middleName = value;
        FullName = PersonHelper.BuildFullName(_firstName, _middleName, _lastName);

        _updatedEvent.MiddleName = new Modification<PersonNameUnit>(value);
        _updatedEvent.FullName = new Modification<string>(FullName);
      }
    }
  }
  private PersonNameUnit? _lastName = null;
  /// <summary>
  /// Gets or sets the last name of the user.
  /// </summary>
  public PersonNameUnit? LastName
  {
    get => _lastName;
    set
    {
      if (value != _lastName)
      {
        _lastName = value;
        FullName = PersonHelper.BuildFullName(_firstName, _middleName, _lastName);

        _updatedEvent.LastName = new Modification<PersonNameUnit>(value);
        _updatedEvent.FullName = new Modification<string>(FullName);
      }
    }
  }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public string? FullName { get; private set; }
  private PersonNameUnit? _nickname = null;
  /// <summary>
  /// Gets or sets the nickname of the user.
  /// </summary>
  public PersonNameUnit? Nickname
  {
    get => _nickname;
    set
    {
      if (value != _nickname)
      {
        _nickname = value;
        _updatedEvent.Nickname = new Modification<PersonNameUnit>(value);
      }
    }
  }

  private readonly BirthdateValidator _birthdateValidator = new(nameof(Birthdate));
  private DateTime? _birthdate = null;
  /// <summary>
  /// Gets or sets the birthdate of the user.
  /// </summary>
  public DateTime? Birthdate
  {
    get => _birthdate;
    set
    {
      if (value != _birthdate)
      {
        if (value.HasValue)
        {
          _birthdateValidator.ValidateAndThrow(value.Value);
        }

        _birthdate = value;
        _updatedEvent.Birthdate = new Modification<DateTime?>(value);
      }
    }
  }
  private GenderUnit? _gender = null;
  /// <summary>
  /// Gets or sets the gender of the user.
  /// </summary>
  public GenderUnit? Gender
  {
    get => _gender;
    set
    {
      if (value != _gender)
      {
        _gender = value;
        _updatedEvent.Gender = new Modification<GenderUnit>(value);
      }
    }
  }
  private LocaleUnit? _locale = null;
  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  public LocaleUnit? Locale
  {
    get => _locale;
    set
    {
      if (value != _locale)
      {
        _locale = value;
        _updatedEvent.Locale = new Modification<LocaleUnit>(value);
      }
    }
  }
  private TimeZoneUnit? _timeZone = null;
  /// <summary>
  /// Gets or sets the time zone of the user.
  /// </summary>
  public TimeZoneUnit? TimeZone
  {
    get => _timeZone;
    set
    {
      if (value != _timeZone)
      {
        _timeZone = value;
        _updatedEvent.TimeZone = new Modification<TimeZoneUnit>(value);
      }
    }
  }

  private UrlUnit? _picture = null;
  /// <summary>
  /// Gets or sets the URL to the picture of the user.
  /// </summary>
  public UrlUnit? Picture
  {
    get => _picture;
    set
    {
      if (value != _picture)
      {
        _picture = value;
        _updatedEvent.Picture = new Modification<UrlUnit>(value);
      }
    }
  }
  private UrlUnit? _profile = null;
  /// <summary>
  /// Gets or sets the URL to the profile page of the user.
  /// </summary>
  public UrlUnit? Profile
  {
    get => _profile;
    set
    {
      if (value != _profile)
      {
        _profile = value;
        _updatedEvent.Profile = new Modification<UrlUnit>(value);
      }
    }
  }
  private UrlUnit? _website = null;
  /// <summary>
  /// Gets or sets the URL to the website of the user.
  /// </summary>
  public UrlUnit? Website
  {
    get => _website;
    set
    {
      if (value != _website)
      {
        _website = value;
        _updatedEvent.Website = new Modification<UrlUnit>(value);
      }
    }
  }

  /// <summary>
  /// Gets of sets the date and time of the latest authentication of this user.
  /// </summary>
  public DateTime? AuthenticatedOn { get; private set; }

  private readonly Dictionary<string, string> _customAttributes = [];
  /// <summary>
  /// Gets the custom attributes of the user.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  private readonly Dictionary<string, string> _customIdentifiers = [];
  /// <summary>
  /// Gets the custom identifiers of the user.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomIdentifiers => _customIdentifiers.AsReadOnly();

  private readonly HashSet<RoleId> _roles = [];
  /// <summary>
  /// Gets the roles of the user.
  /// </summary>
  public IReadOnlyCollection<RoleId> Roles => _roles.ToList().AsReadOnly();

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAggregate"/> class.
  /// DO NOT use this constructor to create a new user. It is only intended to be used by the event sourcing.
  /// </summary>
  /// <param name="id">The identifier of the user.</param>
  public UserAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAggregate"/> class.
  /// DO use this constructor to create a new user.
  /// </summary>
  /// <param name="uniqueName">The unique name of the user.</param>
  /// <param name="tenantId">The tenant identifier of the user.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="id">The identifier of the user.</param>
  public UserAggregate(UniqueNameUnit uniqueName, TenantId? tenantId = null, ActorId actorId = default, UserId? id = null)
    : base((id ?? UserId.NewId()).AggregateId)
  {
    Raise(new UserCreatedEvent(tenantId, uniqueName), actorId);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserCreatedEvent @event)
  {
    TenantId = @event.TenantId;

    _uniqueName = @event.UniqueName;
  }

  /// <summary>
  /// Adds the specified role to the user, if the user does not already have the specified role.
  /// </summary>
  /// <param name="role">The role to be added.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <exception cref="TenantMismatchException">The role and user tenant identifiers do not match.</exception>
  public void AddRole(RoleAggregate role, ActorId actorId = default)
  {
    if (role.TenantId != TenantId)
    {
      throw new TenantMismatchException(TenantId, role.TenantId);
    }

    if (!HasRole(role))
    {
      Raise(new UserRoleAddedEvent(role.Id), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserRoleAddedEvent @event)
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
    Raise(new UserAuthenticatedEvent(), actorId.Value);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserAuthenticatedEvent @event)
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
    Raise(new UserPasswordChangedEvent(newPassword), actorId.Value);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserPasswordChangedEvent @event)
  {
    _password = @event.Password;
  }

  /// <summary>
  /// Deletes the user, if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new UserDeletedEvent(), actorId);
    }
  }

  /// <summary>
  /// Disables the user, if it is enabled.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Disable(ActorId actorId = default)
  {
    if (!IsDisabled)
    {
      Raise(new UserDisabledEvent(), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Apply(UserDisabledEvent _)
  {
    IsDisabled = true;
  }

  /// <summary>
  /// Enables the user, if it is disabled.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Enable(ActorId actorId = default)
  {
    if (IsDisabled)
    {
      Raise(new UserEnabledEvent(), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Apply(UserEnabledEvent _)
  {
    IsDisabled = false;
  }

  /// <summary>
  /// Returns a value indicating whether or not the user has the specified role.
  /// </summary>
  /// <param name="role">The role to match.</param>
  /// <returns>True if the user has the specified role, or false otherwise.</returns>
  public bool HasRole(RoleAggregate role) => HasRole(role.Id);
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
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();

    if (_customAttributes.ContainsKey(key))
    {
      _updatedEvent.CustomAttributes[key] = null;
      _customAttributes.Remove(key);
    }
  }

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

  /// <summary>
  /// Removes the specified role from the user, if the user has the specified role.
  /// </summary>
  /// <param name="role">The role to be removed.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void RemoveRole(RoleAggregate role, ActorId actorId = default)
  {
    if (HasRole(role))
    {
      Raise(new UserRoleRemovedEvent(role.Id), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserRoleRemovedEvent @event)
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
    Raise(new UserPasswordResetEvent(password), actorId.Value);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserPasswordResetEvent @event)
  {
    _password = @event.Password;
  }

  /// <summary>
  /// Sets the postal address of the user.
  /// </summary>
  /// <param name="address">The postal address.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetAddress(AddressUnit? address, ActorId actorId = default)
  {
    if (address != Address)
    {
      Raise(new UserAddressChangedEvent(address), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserAddressChangedEvent @event)
  {
    Address = @event.Address;
  }

  private readonly CustomAttributeValidator _customAttributeValidator = new();
  /// <summary>
  /// Sets the specified custom attribute on the user.
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
  /// Sets the email address of the user.
  /// </summary>
  /// <param name="email">The email address.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetEmail(EmailUnit? email, ActorId actorId = default)
  {
    if (email != Email)
    {
      Raise(new UserEmailChangedEvent(email), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserEmailChangedEvent @event)
  {
    Email = @event.Email;
  }

  /// <summary>
  /// Sets the password of the user.
  /// </summary>
  /// <param name="password">The new password.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetPassword(Password password, ActorId actorId = default)
  {
    Raise(new UserPasswordUpdatedEvent(password), actorId);
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserPasswordUpdatedEvent @event)
  {
    _password = @event.Password;
  }

  /// <summary>
  /// Sets the phone number of the user.
  /// </summary>
  /// <param name="phone">The phone number.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetPhone(PhoneUnit? phone, ActorId actorId = default)
  {
    if (phone != Phone)
    {
      Raise(new UserPhoneChangedEvent(phone), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserPhoneChangedEvent @event)
  {
    Phone = @event.Phone;
  }

  /// <summary>
  /// Sets the unique name of the user.
  /// </summary>
  /// <param name="uniqueName">The unique name.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetUniqueName(UniqueNameUnit uniqueName, ActorId actorId = default)
  {
    if (uniqueName != _uniqueName)
    {
      Raise(new UserUniqueNameChangedEvent(uniqueName), actorId);
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserUniqueNameChangedEvent @event)
  {
    _uniqueName = @event.UniqueName;
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

  /// <summary>
  /// Applies updates on the user.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Update(ActorId actorId = default)
  {
    if (_updatedEvent.HasChanges)
    {
      Raise(_updatedEvent, actorId, DateTime.Now);
      _updatedEvent = new();
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserUpdatedEvent @event)
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
  /// Returns a string representation of the user.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{FullName ?? UniqueName.Value} | {base.ToString()}";
}
