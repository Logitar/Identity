using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using Logitar.Identity.Roles;
using Logitar.Identity.Users.Events;
using Logitar.Identity.Users.Validators;
using System.Globalization;

namespace Logitar.Identity.Users;

/// <summary>
/// The domain aggregate representing an user. Users are the core entity of the identity system. They
/// usually belong to a person and have authentication information. They also contain identification
/// and personal information. Users must belong to a realm. Users can have multiple roles.
/// </summary>
public class UserAggregate : AggregateRoot
{
  /// <summary>
  /// The custom attributes of the user.
  /// </summary>
  private readonly Dictionary<string, string> _customAttributes = new();
  /// <summary>
  /// The external identifiers of the user.
  /// </summary>
  private readonly Dictionary<string, string> _externalIdentifiers = new();
  /// <summary>
  /// The role identifiers of the user.
  /// </summary>
  private readonly List<AggregateId> _roles = new();

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAggregate"/> class using the specified aggregate identifier.
  /// </summary>
  /// <param name="id">The aggregate identifier.</param>
  public UserAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAggregate"/> class using the specified arguments.
  /// </summary>
  /// <param name="actorId">The identifier of the actor creating the user.</param>
  /// <param name="realm">The realm in which the user belongs.</param>
  /// <param name="username">The unique name of the user.</param>
  /// <param name="passwordHash">The salted and hashed password of the user.</param>
  /// <param name="address">The postal address of the user.</param>
  /// <param name="email">The email address of the user.</param>
  /// <param name="phone">The phone number of the user.</param>
  /// <param name="firstName">The first name(s) or given name(s) of the user.</param>
  /// <param name="middleName">The middle name(s) of the user.</param>
  /// <param name="lastName">The last name(s) or surname(s) of the user.</param>
  /// <param name="nickname">The nickname(s) or casual name(s) of the user.</param>
  /// <param name="birthdate">The birthdate of the user.</param>
  /// <param name="gender">The gender of the user.</param>
  /// <param name="locale">The locale of the user.</param>
  /// <param name="timeZone">The time zone of the user. It should match the name of a time zone in the tz database.</param>
  /// <param name="picture">The link (URL) to the picture of the user.</param>
  /// <param name="profile">The link (URL) to the profile of the user.</param>
  /// <param name="website">The link (URL) to the website of the user.</param>
  /// <param name="customAttributes">The custom attributes of the user.</param>
  /// <param name="roles">The roles of the user.</param>
  public UserAggregate(AggregateId actorId, RealmAggregate realm, string username, string? passwordHash = null,
    ReadOnlyAddress? address = null, ReadOnlyEmail? email = null, ReadOnlyPhone? phone = null,
    string? firstName = null, string? middleName = null, string? lastName = null, string? nickname = null,
    DateTime? birthdate = null, Gender? gender = null, CultureInfo? locale = null, string? timeZone = null,
    string? picture = null, string? profile = null, string? website = null,
    Dictionary<string, string>? customAttributes = null, IEnumerable<RoleAggregate>? roles = null) : base()
  {
    UserCreatedEvent e = new()
    {
      ActorId = actorId,
      RealmId = realm.Id,
      Username = username.Trim(),
      PasswordHash = passwordHash,
      Address = address,
      AddressVerification = address?.IsVerified == true ? VerificationAction.Verify : VerificationAction.None,
      Email = email,
      EmailVerification = email?.IsVerified == true ? VerificationAction.Verify : VerificationAction.None,
      Phone = phone,
      PhoneVerification = phone?.IsVerified == true ? VerificationAction.Verify : VerificationAction.None,
      FirstName = firstName?.CleanTrim(),
      MiddleName = middleName?.CleanTrim(),
      LastName = lastName?.CleanTrim(),
      FullName = GetFullName(firstName, middleName, lastName),
      Nickname = nickname?.CleanTrim(),
      Birthdate = birthdate,
      Gender = gender,
      Locale = locale,
      TimeZone = timeZone?.CleanTrim(),
      Picture = picture?.CleanTrim(),
      Profile = profile?.CleanTrim(),
      Website = website?.CleanTrim(),
      CustomAttributes = customAttributes ?? new(),
      Roles = roles?.Select(role => role.Id) ?? Enumerable.Empty<AggregateId>()
    };
    new UserCreatedValidator(realm.UsernameSettings).ValidateAndThrow(e);

    ApplyChange(e);
  }

  /// <summary>
  /// Gets or sets the identifier of the realm in which the user belongs.
  /// </summary>
  public AggregateId RealmId { get; private set; }

  /// <summary>
  /// Gets or sets the unique name of the user.
  /// </summary>
  public string Username { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the salted and hashed password of the user.
  /// </summary>
  public string? PasswordHash { get; private set; }
  /// <summary>
  /// Gets a value indicating whether or not the user has a password.
  /// </summary>
  public bool HasPassword => PasswordHash != null;

  /// <summary>
  /// Gets or sets a value indicating whether or not the user account is disabled.
  /// </summary>
  public bool IsDisabled { get; private set; }

  /// <summary>
  /// Gets or sets the date and time when the user signed-in lastly.
  /// </summary>
  public DateTime? SignedInOn { get; private set; }

  /// <summary>
  /// Gets or sets the postal address of the user.
  /// </summary>
  public ReadOnlyAddress? Address { get; private set; }
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public ReadOnlyEmail? Email { get; private set; }
  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public ReadOnlyPhone? Phone { get; private set; }

  /// <summary>
  /// Gets or sets a value indicating whether or not the user account is confirmed.
  /// </summary>
  public bool IsConfirmed => Address?.IsVerified == true || Email?.IsVerified == true || Phone?.IsVerified == true;

  /// <summary>
  /// Gets or sets the first name(s) or given name(s) of the user.
  /// </summary>
  public string? FirstName { get; private set; }
  /// <summary>
  /// Gets or sets the middle name(s) of the user.
  /// </summary>
  public string? MiddleName { get; private set; }
  /// <summary>
  /// Gets or sets the last name(s) or surname(s) of the user.
  /// </summary>
  public string? LastName { get; private set; }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public string? FullName { get; private set; }
  /// <summary>
  /// Gets or sets the nickname(s) or casual name(s) or the user.
  /// </summary>
  public string? Nickname { get; private set; }

  /// <summary>
  /// Gets or sets the birtdate of the user.
  /// </summary>
  public DateTime? Birthdate { get; private set; }
  /// <summary>
  /// Gets or sets the gender of the user.
  /// </summary>
  public Gender? Gender { get; private set; }

  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  public CultureInfo? Locale { get; private set; }
  /// <summary>
  /// Gets or sets the time zone of the user. It should match the name of a time zone in the tz database.
  /// </summary>
  public string? TimeZone { get; private set; }

  /// <summary>
  /// Gets or sets a link (URL) to the picture of the user.
  /// </summary>
  public string? Picture { get; private set; }
  /// <summary>
  /// Gets or sets a link (URL) to the profile of the user.
  /// </summary>
  public string? Profile { get; private set; }
  /// <summary>
  /// Gets or sets a link (URL) to the website of the user.
  /// </summary>
  public string? Website { get; private set; }

  /// <summary>
  /// Gets the custom attributes of the user.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Gets the external identifiers of the user.
  /// </summary>
  public IReadOnlyDictionary<string, string> ExternalIdentifiers => _externalIdentifiers.AsReadOnly();

  /// <summary>
  /// Gets the role identifiers of the user.
  /// </summary>
  public IReadOnlyCollection<AggregateId> Roles => _roles.AsReadOnly();

  /// <summary>
  /// Applies the specified event to the user.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(UserCreatedEvent e)
  {
    RealmId = e.RealmId;

    Username = e.Username;

    Apply((UserSavedEvent)e);
  }

  /// <summary>
  /// Deletes the user.
  /// </summary>
  /// <param name="actorId">The identifier of the actor deleting the user.</param>
  public void Delete(AggregateId actorId)
  {
    ApplyChange(new UserDeletedEvent
    {
      ActorId = actorId
    });
  }
  /// <summary>
  /// Applies the specified event to the user.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(UserDeletedEvent e)
  {
  }

  /// <summary>
  /// Disables the user account.
  /// </summary>
  /// <param name="actorId">The identifier of the actor disabling the user account.</param>
  /// <returns>False if the user account was already disabled, true otherwise.</returns>
  public bool Disable(AggregateId actorId)
  {
    if (IsDisabled)
    {
      return false;
    }

    ApplyChange(new UserDisabledEvent
    {
      ActorId = actorId
    });

    return true;
  }
  /// <summary>
  /// Applies the specified event to the user.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(UserDisabledEvent e)
  {
    IsDisabled = true;
  }

  /// <summary>
  /// Enables the user account.
  /// </summary>
  /// <param name="actorId">The identifier of the actor enabling the user account.</param>
  /// <returns>False if the user account was not disabled, true otherwise.</returns>
  public bool Enable(AggregateId actorId)
  {
    if (!IsDisabled)
    {
      return false;
    }

    ApplyChange(new UserEnabledEvent
    {
      ActorId = actorId
    });

    return true;
  }
  /// <summary>
  /// Applies the specified event to the user.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(UserEnabledEvent e)
  {
    IsDisabled = false;
  }

  /// <summary>
  /// Saves the specified external identifier to the user.
  /// </summary>
  /// <param name="actorId">The identifier of the actor adding the external identifier.</param>
  /// <param name="key">The key of the external identifier.</param>
  /// <param name="value">The value of the external identifier. If null, the external identifier will be removed.</param>
  public void SaveExternalIdentifier(AggregateId actorId, string key, string? value)
  {
    if (value == null && !_externalIdentifiers.ContainsKey(key))
    {
      throw new ExternalIdentifierNotFoundException(this, key);
    }

    ExternalIdentifierSavedEvent e = new()
    {
      ActorId = actorId,
      Key = key.Trim(),
      Value = value?.CleanTrim()
    };
    new ExternalIdentifierSavedValidator().ValidateAndThrow(e);

    ApplyChange(e);
  }
  /// <summary>
  /// Applies the specified event to the user.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(ExternalIdentifierSavedEvent e)
  {
    if (e.Value == null)
    {
      _externalIdentifiers.Remove(e.Key);
    }
    else
    {
      _externalIdentifiers[e.Key] = e.Value;
    }
  }

  /// <summary>
  /// Signs-in the user at the specified date and time.
  /// </summary>
  /// <param name="signedInOn">The date and time when the user signed-in.</param>
  public void SignIn(DateTime signedInOn)
  {
    ApplyChange(new UserSignedInEvent
    {
      ActorId = Id,
      OccurredOn = signedInOn
    });
  }
  /// <summary>
  /// Applies the specified event to the user.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(UserSignedInEvent e)
  {
    SignedInOn = e.OccurredOn;
  }

  /// <summary>
  /// Updates the user.
  /// </summary>
  /// <param name="actorId">The identifier of the actor creating the user.</param>
  /// <param name="passwordHash">The salted and hashed password of the user. If null, the password won't be changed.</param>
  /// <param name="address">The postal address of the user.</param>
  /// <param name="email">The email address of the user.</param>
  /// <param name="phone">The phone number of the user.</param>
  /// <param name="firstName">The first name(s) or given name(s) of the user.</param>
  /// <param name="middleName">The middle name(s) of the user.</param>
  /// <param name="lastName">The last name(s) or surname(s) of the user.</param>
  /// <param name="nickname">The nickname(s) or casual name(s) of the user.</param>
  /// <param name="birthdate">The birthdate of the user.</param>
  /// <param name="gender">The gender of the user.</param>
  /// <param name="locale">The locale of the user.</param>
  /// <param name="timeZone">The time zone of the user. It should match the name of a time zone in the tz database.</param>
  /// <param name="picture">The link (URL) to the picture of the user.</param>
  /// <param name="profile">The link (URL) to the profile of the user.</param>
  /// <param name="website">The link (URL) to the website of the user.</param>
  /// <param name="customAttributes">The custom attributes of the user.</param>
  /// <param name="roles">The roles of the user.</param>
  public void Update(AggregateId actorId, string? passwordHash,
    ReadOnlyAddress? address, ReadOnlyEmail? email, ReadOnlyPhone? phone,
    string? firstName, string? middleName, string? lastName, string? nickname,
    DateTime? birthdate, Gender? gender, CultureInfo? locale,
    string? timeZone, string? picture, string? profile, string? website,
    Dictionary<string, string>? customAttributes, IEnumerable<RoleAggregate>? roles)
  {
    bool isAddressModified = Address?.Line1 != address?.Line1 || Address?.Line2 != address?.Line2
      || Address?.Locality != address?.Locality || Address?.PostalCode != address?.PostalCode
      || Address?.Country != address?.Country || Address?.Region != address?.Region;
    VerificationAction addressVerification = email?.IsVerified == true ? VerificationAction.Verify
      : (isAddressModified ? VerificationAction.Unverify : VerificationAction.None);
    bool isEmailModified = Email?.Address != email?.Address;
    VerificationAction emailVerification = email?.IsVerified == true ? VerificationAction.Verify
      : (isEmailModified ? VerificationAction.Unverify : VerificationAction.None);
    bool isPhoneModified = Phone?.CountryCode != phone?.CountryCode
      || Phone?.Number != phone?.Number || Phone?.Extension != phone?.Extension;
    VerificationAction phoneVerification = phone?.IsVerified == true ? VerificationAction.Verify
      : (isPhoneModified ? VerificationAction.Unverify : VerificationAction.None);

    UserUpdatedEvent e = new()
    {
      ActorId = actorId,
      PasswordHash = passwordHash,
      Address = address,
      AddressVerification = addressVerification,
      Email = email,
      EmailVerification = emailVerification,
      Phone = phone,
      PhoneVerification = phoneVerification,
      FirstName = firstName?.CleanTrim(),
      MiddleName = middleName?.CleanTrim(),
      LastName = lastName?.CleanTrim(),
      FullName = GetFullName(firstName, middleName, lastName),
      Nickname = nickname?.CleanTrim(),
      Birthdate = birthdate,
      Gender = gender,
      Locale = locale,
      TimeZone = timeZone?.CleanTrim(),
      Picture = picture?.CleanTrim(),
      Profile = profile?.CleanTrim(),
      Website = website?.CleanTrim(),
      CustomAttributes = customAttributes ?? new(),
      Roles = roles?.Select(role => role.Id) ?? Enumerable.Empty<AggregateId>()
    };
    new UserUpdatedValidator().ValidateAndThrow(e);

    ApplyChange(e);
  }
  /// <summary>
  /// Applies the specified event to the user.
  /// </summary>
  /// <param name="e">The domain event.</param>
  protected virtual void Apply(UserUpdatedEvent e)
  {
    Apply((UserSavedEvent)e);
  }

  /// <summary>
  /// Applies the specified event to the realm.
  /// </summary>
  /// <param name="e">The domain event.</param>
  private void Apply(UserSavedEvent e)
  {
    if (e.PasswordHash != null)
    {
      PasswordHash = e.PasswordHash;
    }

    Address = e.Address;
    Email = e.Email;
    Phone = e.Phone;

    FirstName = e.FirstName;
    MiddleName = e.MiddleName;
    LastName = e.LastName;
    FullName = e.FullName;
    Nickname = e.Nickname;

    Birthdate = e.Birthdate;
    Gender = e.Gender;

    Locale = e.Locale;
    TimeZone = e.TimeZone;

    Picture = e.Picture;
    Profile = e.Profile;
    Website = e.Website;

    _customAttributes.Clear();
    _customAttributes.AddRange(e.CustomAttributes);

    _roles.Clear();
    _roles.AddRange(e.Roles);
  }

  /// <summary>
  /// Returns a string representation of the current user.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{Username} | {base.ToString()}";

  /// <summary>
  /// Builds the full name of an user using its list of names.
  /// </summary>
  /// <param name="names">The list of names of the user.</param>
  /// <returns>The full name of the user.</returns>
  private static string? GetFullName(params string?[] names)
  {
    return string.Join(' ', names.Where(name => !string.IsNullOrWhiteSpace(name))
      .SelectMany(name => name!.Split())
      .Where(name => !string.IsNullOrEmpty(name))).CleanTrim();
  }
}
