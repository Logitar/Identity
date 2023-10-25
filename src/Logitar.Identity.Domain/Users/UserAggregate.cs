using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents an user in the identity system. An user generally represents the account of a person.
/// It contains personal information about that person as well as authentication information that could be used to authenticate that person.
/// </summary>
public class UserAggregate : AggregateRoot
{
  private readonly Dictionary<string, string> _customAttributes = new();
  private readonly HashSet<RoleId> _roles = new();
  private Password? _password = null;
  private UserUpdatedEvent _updated = new();

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
  public UniqueNameUnit UniqueName => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");

  /// <summary>
  /// Gets or sets a value indicating whether or not the user is disabled.
  /// </summary>
  public bool IsDisabled { get; private set; }

  private AddressUnit? _address = null;
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public AddressUnit? Address
  {
    get => _address;
    set
    {
      if (value != _address)
      {
        _updated.Address = new Modification<AddressUnit>(value);
        _address = value;
      }
    }
  }
  private EmailUnit? _email = null;
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public EmailUnit? Email
  {
    get => _email;
    set
    {
      if (value != _email)
      {
        _updated.Email = new Modification<EmailUnit>(value);
        _email = value;
      }
    }
  }
  private PhoneUnit? _phone = null;
  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public PhoneUnit? Phone
  {
    get => _phone;
    set
    {
      if (value != _phone)
      {
        _updated.Phone = new Modification<PhoneUnit>(value);
        _phone = value;
      }
    }
  }
  /// <summary>
  /// Gets a value indicating whether or not the user is confirmed.
  /// A confirmed user has at least one verified contact information.
  /// </summary>
  public bool IsConfirmed => _address?.IsVerified == true || _email?.IsVerified == true || _phone?.IsVerified == true;

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
        _updated.FirstName = new Modification<PersonNameUnit>(value);
        _updated.FullName = new Modification<string>(BuildFullName(value, MiddleName, LastName));

        _firstName = value;
        FullName = _updated.FullName.Value;
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
        _updated.MiddleName = new Modification<PersonNameUnit>(value);
        _updated.FullName = new Modification<string>(BuildFullName(FirstName, value, LastName));

        _middleName = value;
        FullName = _updated.FullName.Value;
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
        _updated.LastName = new Modification<PersonNameUnit>(value);
        _updated.FullName = new Modification<string>(BuildFullName(FirstName, MiddleName, value));

        _lastName = value;
        FullName = _updated.FullName.Value;
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
        _updated.Nickname = new Modification<PersonNameUnit>(value);
        _nickname = value;
      }
    }
  }

  private DateTime? _birthdate = null;
  /// <summary>
  /// Gets or sets the birthdate of the user.
  /// </summary>
  public DateTime? Birthdate
  {
    get => _birthdate;
    set
    {
      if (value.HasValue)
      {
        new BirthdateValidator(nameof(Birthdate)).ValidateAndThrow(value.Value);
      }

      if (value != _birthdate)
      {
        _updated.Birthdate = new Modification<DateTime?>(value);
        _birthdate = value;
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
        _updated.Gender = new Modification<GenderUnit>(value);
        _gender = value;
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
        _updated.Locale = new Modification<LocaleUnit>(value);
        _locale = value;
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
        _updated.TimeZone = new Modification<TimeZoneUnit>(value);
        _timeZone = value;
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
        _updated.Picture = new Modification<UrlUnit>(value);
        _picture = value;
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
        _updated.Profile = new Modification<UrlUnit>(value);
        _profile = value;
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
        _updated.Website = new Modification<UrlUnit>(value);
        _website = value;
      }
    }
  }

  /// <summary>
  /// Gets the custom attributes of the user.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

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
    : base(id?.AggregateId)
  {
    ApplyChange(new UserCreatedEvent(actorId, uniqueName, tenantId));
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
      ApplyChange(new UserRoleAddedEvent(actorId, role.Id));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserRoleAddedEvent @event) => _roles.Add(@event.RoleId);

  /// <summary>
  /// Authenticates the user.
  /// </summary>
  /// <param name="password">The current password of the user.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <exception cref="IncorrectUserPasswordException">The password is incorrect.</exception>
  /// <exception cref="UserIsDisabledException">The user is disabled.</exception>
  public void Authenticate(string password, string? propertyName = null, ActorId? actorId = null)
  {
    if (_password?.IsMatch(password) != true)
    {
      throw new IncorrectUserPasswordException(password, this, propertyName);
    }
    else if (IsDisabled)
    {
      throw new UserIsDisabledException(this);
    }

    actorId ??= new(Id.Value);
    ApplyChange(new UserAuthenticatedEvent(actorId.Value));
  }

  /// <summary>
  /// Changes the password of the user, validating its current password.
  /// </summary>
  /// <param name="currentPassword">The current password of the user.</param>
  /// <param name="newPassword">The new password of the user.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  /// <exception cref="IncorrectUserPasswordException">The current password is incorrect.</exception>
  public void ChangePassword(string currentPassword, Password newPassword, string? propertyName = null, ActorId? actorId = null)
  {
    if (_password?.IsMatch(currentPassword) != true)
    {
      throw new IncorrectUserPasswordException(currentPassword, this, propertyName);
    }

    actorId ??= new(Id.Value);
    ApplyChange(new UserPasswordChangedEvent(actorId.Value, newPassword));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserPasswordChangedEvent @event) => _password = @event.Password;

  /// <summary>
  /// Deletes the user, if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      ApplyChange(new UserDeletedEvent(actorId));
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
      ApplyChange(new UserDisabledEvent(actorId));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Apply(UserDisabledEvent _) => IsDisabled = true;

  /// <summary>
  /// Enables the user, if it is disabled.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Enable(ActorId actorId = default)
  {
    if (IsDisabled)
    {
      ApplyChange(new UserEnabledEvent(actorId));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Apply(UserEnabledEvent _) => IsDisabled = false;

  /// <summary>
  /// Returns a value indicating whether or not the user has the specified role.
  /// </summary>
  /// <param name="role">The role to match.</param>
  /// <returns>True if the user has the specified role, or false otherwise.</returns>
  public bool HasRole(RoleAggregate role) => _roles.Contains(role.Id);

  /// <summary>
  /// Removes the specified custom attribute on the user.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();

    if (_customAttributes.ContainsKey(key))
    {
      _updated.CustomAttributes[key] = null;
      _customAttributes.Remove(key);
    }
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
      ApplyChange(new UserRoleRemovedEvent(actorId, role.Id));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserRoleRemovedEvent @event) => _roles.Remove(@event.RoleId);

  /// <summary>
  /// Resets the password of the user.
  /// </summary>
  /// <param name="password">The new password of the user.</param>
  /// <param name="actorId">(Optional) The actor identifier. This parameter should be left null so that it defaults to the user's identifier.</param>
  public void ResetPassword(Password password, ActorId? actorId = null)
  {
    actorId ??= new(Id.Value);
    ApplyChange(new UserPasswordResetEvent(actorId.Value, password));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserPasswordResetEvent @event) => _password = @event.Password;

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
      _updated.CustomAttributes[key] = value;
      _customAttributes[key] = value;
    }
  }

  /// <summary>
  /// Sets the password of the user.
  /// </summary>
  /// <param name="password">The new password.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetPassword(Password password, ActorId actorId = default)
  {
    ApplyChange(new UserPasswordSetEvent(actorId, password));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserPasswordSetEvent @event) => _password = @event.Password;

  /// <summary>
  /// Sets the unique name of the user.
  /// </summary>
  /// <param name="uniqueName">The unique name.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetUniqueName(UniqueNameUnit uniqueName, ActorId actorId = default)
  {
    if (uniqueName != _uniqueName)
    {
      ApplyChange(new UserUniqueNameChangedEvent(actorId, uniqueName));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserUniqueNameChangedEvent @event) => _uniqueName = @event.UniqueName;

  /// <summary>
  /// Applies updates on the user.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Update(ActorId actorId = default)
  {
    if (_updated.HasChanges)
    {
      _updated.ActorId = actorId;

      ApplyChange(_updated);

      _updated = new();
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserUpdatedEvent @event)
  {
    if (@event.Address != null)
    {
      _address = @event.Address.Value;
    }
    if (@event.Email != null)
    {
      _email = @event.Email.Value;
    }
    if (@event.Phone != null)
    {
      _phone = @event.Phone.Value;
    }

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

  private static string? BuildFullName(params PersonNameUnit?[] names) => string.Join(' ', names
    .SelectMany(name => name?.Value.Split() ?? Array.Empty<string>())
    .Where(name => !string.IsNullOrEmpty(name))).CleanTrim();
}
