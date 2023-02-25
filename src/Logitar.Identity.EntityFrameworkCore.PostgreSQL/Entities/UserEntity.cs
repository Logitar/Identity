using Logitar.EventSourcing;
using Logitar.Identity.Users;
using Logitar.Identity.Users.Events;
using System.Text.Json;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing an user.
/// </summary>
internal class UserEntity : AggregateEntity, ICustomAttributes
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserEntity"/> class using the specified arguments.
  /// </summary>
  /// <param name="e">The creation event.</param>
  /// <param name="realm">The realm the user belongs to.</param>
  /// <param name="actor">The actor creating the user.</param>
  public UserEntity(UserCreatedEvent e, RealmEntity realm, ActorEntity actor) : base(e, actor)
  {
    Realm = realm;
    RealmId = realm.RealmId;

    Username = e.Username;

    Apply(e, actor);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="UserEntity"/> class.
  /// </summary>
  private UserEntity()
  {
  }

  /// <summary>
  /// Gets or sets the primary identifier of the user.
  /// </summary>
  public int UserId { get; private set; }

  /// <summary>
  /// Gets or sets the realm in which the user belongs.
  /// </summary>
  public RealmEntity? Realm { get; private set; }
  /// <summary>
  /// Gets or sets or sets the identifier of the realm in which the user belongs.
  /// </summary>
  public int RealmId { get; private set; }

  /// <summary>
  /// Gets or sets the unique name of the user.
  /// </summary>
  public string Username { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the normalized unique name of the user for unicity purposes.
  /// </summary>
  public string UsernameNormalized
  {
    get => Username.ToUpper();
    private set { }
  }
  /// <summary>
  /// Gets or sets the salted and hashed password of the user.
  /// </summary>
  public string? PasswordHash { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the actor who changed the user's password lastly.
  /// </summary>
  public string? PasswordChangedById { get; private set; }
  /// <summary>
  /// Gets or sets the serialized actor changed the password lastly.
  /// </summary>
  public string? PasswordChangedBy { get; private set; }
  /// <summary>
  /// Gets or sets the date and time when the password changed lastly.
  /// </summary>
  public DateTime? PasswordChangedOn { get; private set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the user has a password.
  /// </summary>
  public bool HasPassword { get; private set; }

  /// <summary>
  /// Gets or sets the identifier of the actor who disabled the user account.
  /// </summary>
  public string? DisabledById { get; private set; }
  /// <summary>
  /// Gets or sets the serialized actor who disabled the user account.
  /// </summary>
  public string? DisabledBy { get; private set; }
  /// <summary>
  /// Gets or sets the date and time when the user account was disabled.
  /// </summary>
  public DateTime? DisabledOn { get; private set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the user account is disabled.
  /// </summary>
  public bool IsDisabled { get; private set; }

  /// <summary>
  /// Gets or sets the date and time when the user signed-in lastly.
  /// </summary>
  public DateTime? SignedInOn { get; private set; }

  /// <summary>
  /// Gets or sets the primary line of the user's postal address.
  /// </summary>
  public string? AddressLine1 { get; private set; }
  /// <summary>
  /// Gets or sets the secondary line of the user's postal address.
  /// </summary>
  public string? AddressLine2 { get; private set; }
  /// <summary>
  /// Gets or sets the locality of the user's postal address.
  /// </summary>
  public string? AddressLocality { get; private set; }
  /// <summary>
  /// Gets or sets the postal code of the user's postal address.
  /// </summary>
  public string? AddressPostalCode { get; private set; }
  /// <summary>
  /// Gets or sets the country of the user's postal address.
  /// </summary>
  public string? AddressCountry { get; private set; }
  /// <summary>
  /// Gets or sets the region of the user's postal address.
  /// </summary>
  public string? AddressRegion { get; private set; }
  /// <summary>
  /// Gets or sets the formatted user's postal address.
  /// </summary>
  public string? AddressFormatted { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the actor who verified the postal address.
  /// </summary>
  public string? AddressVerifiedById { get; private set; }
  /// <summary>
  /// Gets or sets the serialized actor who verified the postal address.
  /// </summary>
  public string? AddressVerifiedBy { get; private set; }
  /// <summary>
  /// Gets or sets the date and time when the postal address was verified.
  /// </summary>
  public DateTime? AddressVerifiedOn { get; private set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the postal address is verified.
  /// </summary>
  public bool IsAddressVerified { get; private set; }

  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public string? EmailAddress { get; private set; }
  /// <summary>
  /// Gets or sets the normalized value of the user's email address.
  /// </summary>
  public string? EmailAddressNormalized { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the actor who verified the email address.
  /// </summary>
  public string? EmailVerifiedById { get; private set; }
  /// <summary>
  /// Gets or sets the serialized actor who verified the email address.
  /// </summary>
  public string? EmailVerifiedBy { get; private set; }
  /// <summary>
  /// Gets or sets the date and time when the email address was verified.
  /// </summary>
  public DateTime? EmailVerifiedOn { get; private set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the email address is verified.
  /// </summary>
  public bool IsEmailVerified { get; private set; }

  /// <summary>
  /// Gets or sets the country code of the user's phone number.
  /// </summary>
  public string? PhoneCountryCode { get; set; }
  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public string? PhoneNumber { get; private set; }
  /// <summary>
  /// Gets or sets the extension of the user's phone number.
  /// </summary>
  public string? PhoneExtension { get; set; }
  /// <summary>
  /// Gets or sets the E.164 formatted user's phone number.
  /// </summary>
  public string? PhoneE164Formatted { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the actor who verified the phone number.
  /// </summary>
  public string? PhoneVerifiedById { get; private set; }
  /// <summary>
  /// Gets or sets the serialized actor who verified the phone number.
  /// </summary>
  public string? PhoneVerifiedBy { get; private set; }
  /// <summary>
  /// Gets or sets the date and time when the phone number was verified.
  /// </summary>
  public DateTime? PhoneVerifiedOn { get; private set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the phone number is verified.
  /// </summary>
  public bool IsPhoneVerified { get; private set; }

  /// <summary>
  /// Gets or sets a value indicating whether or not the user account is confirmed.
  /// </summary>
  public bool IsConfirmed
  {
    get => IsAddressVerified || IsEmailVerified || IsPhoneVerified;
    private set { }
  }

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
  public string? Gender { get; private set; }

  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  public string? Locale { get; private set; }
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
  /// Gets or sets the custom attributes of the user.
  /// </summary>
  public string? CustomAttributes { get; private set; }

  /// <summary>
  /// Gets or sets the list of external identifiers of the user.
  /// </summary>
  public List<ExternalIdentifierEntity> ExternalIdentifiers { get; private set; } = new();

  /// <summary>
  /// Gets or sets the list of roles of the user.
  /// </summary>
  public List<RoleEntity> Roles { get; private set; } = new();

  /// <summary>
  /// Disables the user account to the state of the specified event.
  /// </summary>
  /// <param name="e">The disable event.</param>
  /// <param name="actor">The actor disabling the user account.</param>
  public void Disable(UserDisabledEvent e, ActorEntity actor)
  {
    SetVersion(e);

    DisabledById = e.ActorId.Value;
    DisabledBy = actor.Serialize();
    DisabledOn = e.OccurredOn;
    IsDisabled = true;
  }

  /// <summary>
  /// Enables the user account to the state of the specified event.
  /// </summary>
  /// <param name="e">The enable event.</param>
  /// <param name="actor">The actor enabling the user account.</param>
  public void Enable(UserEnabledEvent e, ActorEntity actor)
  {
    Update(e, actor);

    DisabledById = null;
    DisabledBy = null;
    DisabledOn = null;
    IsDisabled = false;
  }

  /// <summary>
  /// Adds, removes or updates an external identifier of the user.
  /// </summary>
  /// <param name="e">The external identifier event.</param>
  /// <param name="actor">The actor saving the external identifier.</param>
  public void SaveExternalIdentifier(ExternalIdentifierSavedEvent e, ActorEntity actor)
  {
    ExternalIdentifierEntity? externalIdentifier = ExternalIdentifiers.SingleOrDefault(x => x.Key == e.Key);

    if (e.Value == null)
    {
      if (externalIdentifier != null)
      {
        ExternalIdentifiers.Remove(externalIdentifier);
      }
    }
    else if (externalIdentifier == null)
    {
      externalIdentifier = new ExternalIdentifierEntity(e, this, actor);
      ExternalIdentifiers.Add(externalIdentifier);
    }
    else
    {
      externalIdentifier.Update(e, actor);
    }
  }

  /// <summary>
  /// Updates the user to the state of the specified event.
  /// </summary>
  /// <param name="e">The update event.</param>
  /// <param name="actor">The actor updating the user.</param>
  public void Update(UserUpdatedEvent e, ActorEntity actor)
  {
    base.Update(e, actor);

    Apply(e, actor);
  }

  /// <summary>
  /// Update the actors of the user.
  /// </summary>
  /// <param name="id">The identifier of the actor.</param>
  /// <param name="actor">The JSON serialized actor.</param>
  public override void UpdateActors(string id, string actor)
  {
    base.UpdateActors(id, actor);

    if (PasswordChangedById == id)
    {
      PasswordChangedBy = actor;
    }

    if (DisabledById == id)
    {
      DisabledBy = actor;
    }

    if (AddressVerifiedById != id)
    {
      AddressVerifiedBy = actor;
    }

    if (EmailVerifiedById != id)
    {
      EmailVerifiedBy = actor;
    }

    if (PhoneVerifiedById == id)
    {
      PhoneVerifiedBy = actor;
    }
  }

  /// <summary>
  /// Applies the specified event to the user.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  /// <param name="actor">The actor saving the user.</param>
  private void Apply(UserSavedEvent e, ActorEntity actor)
  {
    SetPassword(e, e.PasswordHash, actor);

    AddressLine1 = e.Address?.Line1;
    AddressLine2 = e.Address?.Line2;
    AddressLocality = e.Address?.Locality;
    AddressPostalCode = e.Address?.PostalCode;
    AddressCountry = e.Address?.Country;
    AddressRegion = e.Address?.Region;
    AddressFormatted = e.Address?.ToFormattedString();
    switch (e.AddressVerification)
    {
      case VerificationAction.Verify:
        AddressVerifiedById = e.ActorId.Value;
        AddressVerifiedBy = actor.Serialize();
        AddressVerifiedOn = e.OccurredOn;
        IsAddressVerified = true;
        break;
      case VerificationAction.Unverify:
        AddressVerifiedById = null;
        AddressVerifiedBy = null;
        AddressVerifiedOn = null;
        IsAddressVerified = false;
        break;
    }

    EmailAddress = e.Email?.Address;
    EmailAddressNormalized = e.Email?.Address.ToUpper();
    switch (e.EmailVerification)
    {
      case VerificationAction.Verify:
        EmailVerifiedById = e.ActorId.Value;
        EmailVerifiedBy = actor.Serialize();
        EmailVerifiedOn = e.OccurredOn;
        IsEmailVerified = true;
        break;
      case VerificationAction.Unverify:
        EmailVerifiedById = null;
        EmailVerifiedBy = null;
        EmailVerifiedOn = null;
        IsEmailVerified = false;
        break;
    }

    PhoneCountryCode = e.Phone?.CountryCode;
    PhoneNumber = e.Phone?.Number;
    PhoneExtension = e.Phone?.Extension;
    PhoneE164Formatted = e.Phone?.ToE164String();
    switch (e.PhoneVerification)
    {
      case VerificationAction.Verify:
        PhoneVerifiedById = e.ActorId.Value;
        PhoneVerifiedBy = actor.Serialize();
        PhoneVerifiedOn = e.OccurredOn;
        IsPhoneVerified = true;
        break;
      case VerificationAction.Unverify:
        PhoneVerifiedById = null;
        PhoneVerifiedBy = null;
        PhoneVerifiedOn = null;
        IsPhoneVerified = false;
        break;
    }

    FirstName = e.FirstName;
    MiddleName = e.MiddleName;
    LastName = e.LastName;
    FullName = e.FullName;
    Nickname = e.Nickname;

    Birthdate = e.Birthdate;
    Gender = e.Gender?.Value;

    Locale = e.Locale?.Name;
    TimeZone = e.TimeZone;

    Picture = e.Picture;
    Profile = e.Profile;
    Website = e.Website;

    CustomAttributes = e.CustomAttributes.Any() ? JsonSerializer.Serialize(e.CustomAttributes) : null;
  }

  /// <summary>
  /// Sets the password the user.
  /// </summary>
  /// <param name="e">The password change event.</param>
  /// <param name="passwordHash">The new password the user.</param>
  /// <param name="actor">The actor setting the user's password.</param>
  private void SetPassword(DomainEvent e, string? passwordHash, ActorEntity actor)
  {
    if (passwordHash != null)
    {
      PasswordHash = passwordHash;
      PasswordChangedById = e.ActorId.Value;
      PasswordChangedBy = actor.Serialize();
      PasswordChangedOn = e.OccurredOn;
      HasPassword = true;
    }
  }
}
