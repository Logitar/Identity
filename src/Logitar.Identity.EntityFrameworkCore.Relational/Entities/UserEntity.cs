using Logitar.EventSourcing;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class UserEntity : AggregateEntity
{
  public int UserId { get; private set; }

  public string? TenantId { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => UniqueName.ToUpper();
    private set { }
  }

  public string? PasswordHash { get; private set; }
  public string? PasswordChangedBy { get; private set; }
  public DateTime? PasswordChangedOn { get; private set; }
  public bool HasPassword
  {
    get => PasswordChangedBy != null && PasswordChangedOn != null;
    private set { }
  }

  public string? DisabledBy { get; private set; }
  public DateTime? DisabledOn { get; private set; }
  public bool IsDisabled
  {
    get => DisabledBy != null && DisabledOn != null;
    private set { }
  }

  public string? AddressStreet { get; private set; }
  public string? AddressLocality { get; private set; }
  public string? AddressPostalCode { get; private set; }
  public string? AddressRegion { get; private set; }
  public string? AddressCountry { get; private set; }
  public string? AddressFormatted { get; private set; }
  public string? AddressVerifiedBy { get; private set; }
  public DateTime? AddressVerifiedOn { get; private set; }
  public bool IsAddressVerified
  {
    get => AddressVerifiedBy != null && AddressVerifiedOn != null;
    private set { }
  }

  public string? EmailAddress { get; private set; }
  public string? EmailAddressNormalized
  {
    get => EmailAddress?.ToUpper();
    private set { }
  }
  public string? EmailVerifiedBy { get; private set; }
  public DateTime? EmailVerifiedOn { get; private set; }
  public bool IsEmailVerified
  {
    get => EmailVerifiedBy != null && EmailVerifiedOn != null;
    private set { }
  }

  public string? PhoneCountryCode { get; private set; }
  public string? PhoneNumber { get; private set; }
  public string? PhoneExtension { get; private set; }
  public string? PhoneE164Formatted { get; private set; }
  public string? PhoneVerifiedBy { get; private set; }
  public DateTime? PhoneVerifiedOn { get; private set; }
  public bool IsPhoneVerified
  {
    get => PhoneVerifiedBy != null && PhoneVerifiedOn != null;
    private set { }
  }

  public bool IsConfirmed
  {
    get => IsAddressVerified || IsEmailVerified || IsPhoneVerified;
    private set { }
  }

  public string? FirstName { get; private set; }
  public string? MiddleName { get; private set; }
  public string? LastName { get; private set; }
  public string? FullName { get; private set; }
  public string? Nickname { get; private set; }

  public DateTime? Birthdate { get; private set; }
  public string? Gender { get; private set; }
  public string? Locale { get; private set; }
  public string? TimeZone { get; private set; }

  public string? Picture { get; private set; }
  public string? Profile { get; private set; }
  public string? Website { get; private set; }

  public DateTime? AuthenticatedOn { get; private set; }

  public Dictionary<string, string> CustomAttributes { get; private set; } = [];
  public string? CustomAttributesSerialized
  {
    get => CustomAttributes.Count == 0 ? null : JsonSerializer.Serialize(CustomAttributes);
    private set
    {
      if (value == null)
      {
        CustomAttributes.Clear();
      }
      else
      {
        CustomAttributes = JsonSerializer.Deserialize<Dictionary<string, string>>(value) ?? [];
      }
    }
  }

  public List<UserIdentifierEntity> Identifiers { get; private set; } = [];
  public List<RoleEntity> Roles { get; private set; } = [];
  public List<SessionEntity> Sessions { get; private set; } = [];

  public UserEntity(UserCreatedEvent @event) : base(@event)
  {
    TenantId = @event.TenantId?.Value;

    UniqueName = @event.UniqueName.Value;
  }

  private UserEntity() : base()
  {
  }

  public override IEnumerable<ActorId> GetActorIds() => GetActorIds(skipRoles: false, skipSessions: false);
  public IEnumerable<ActorId> GetActorIds(bool skipRoles, bool skipSessions)
  {
    List<ActorId> actorIds = [];
    actorIds.AddRange(base.GetActorIds());

    if (PasswordChangedBy != null)
    {
      actorIds.Add(new ActorId(PasswordChangedBy));
    }

    if (DisabledBy != null)
    {
      actorIds.Add(new ActorId(DisabledBy));
    }

    if (AddressVerifiedBy != null)
    {
      actorIds.Add(new ActorId(AddressVerifiedBy));
    }
    if (EmailVerifiedBy != null)
    {
      actorIds.Add(new ActorId(EmailVerifiedBy));
    }
    if (PhoneVerifiedBy != null)
    {
      actorIds.Add(new ActorId(PhoneVerifiedBy));
    }

    if (!skipRoles)
    {
      foreach (RoleEntity role in Roles)
      {
        actorIds.AddRange(role.GetActorIds());
      }
    }

    if (!skipSessions)
    {
      foreach (SessionEntity session in Sessions)
      {
        actorIds.AddRange(session.GetActorIds(skipUser: true));
      }
    }

    return actorIds.AsReadOnly();
  }

  public void AddRole(RoleEntity role, UserRoleAddedEvent @event)
  {
    Update(@event);

    Roles.Add(role);
  }

  public void Authenticate(UserAuthenticatedEvent @event)
  {
    Update(@event);

    AuthenticatedOn = @event.OccurredOn;
  }

  public void Disable(UserDisabledEvent @event)
  {
    Update(@event);

    DisabledBy = @event.ActorId.Value;
    DisabledOn = @event.OccurredOn.ToUniversalTime();
  }

  public void Enable(UserEnabledEvent @event)
  {
    Update(@event);

    DisabledBy = null;
    DisabledOn = null;
  }

  public void RemoveCustomIdentifier(UserIdentifierRemovedEvent @event)
  {
    UserIdentifierEntity? identifier = Identifiers.SingleOrDefault(x => x.Key == @event.Key);
    if (identifier != null)
    {
      Identifiers.Remove(identifier);
    }
  }

  public void RemoveRole(UserRoleRemovedEvent @event)
  {
    Update(@event);

    RoleEntity? role = Roles.SingleOrDefault(x => x.AggregateId == @event.RoleId.AggregateId.Value);
    if (role != null)
    {
      Roles.Remove(role);
    }
  }

  public void SetAddress(UserAddressChangedEvent @event)
  {
    Update(@event);

    AddressStreet = @event.Address?.Street;
    AddressLocality = @event.Address?.Locality;
    AddressPostalCode = @event.Address?.PostalCode;
    AddressRegion = @event.Address?.Region;
    AddressCountry = @event.Address?.Country;
    AddressFormatted = @event.Address?.Format();

    if (!IsAddressVerified && @event.Address?.IsVerified == true)
    {
      AddressVerifiedBy = @event.ActorId.Value;
      AddressVerifiedOn = @event.OccurredOn.ToUniversalTime();
    }
    else if (IsAddressVerified && @event.Address?.IsVerified != true)
    {
      AddressVerifiedBy = null;
      AddressVerifiedOn = null;
    }
  }

  public void SetCustomIdentifier(UserIdentifierChangedEvent @event)
  {
    UserIdentifierEntity? identifier = Identifiers.SingleOrDefault(x => x.Key == @event.Key);
    if (identifier == null)
    {
      identifier = new(this, @event);
      Identifiers.Add(identifier);
    }
    else
    {
      identifier.Update(@event);
    }
  }

  public void SetEmail(UserEmailChangedEvent @event)
  {
    Update(@event);

    EmailAddress = @event.Email?.Address;

    if (!IsEmailVerified && @event.Email?.IsVerified == true)
    {
      EmailVerifiedBy = @event.ActorId.Value;
      EmailVerifiedOn = @event.OccurredOn.ToUniversalTime();
    }
    else if (IsEmailVerified && @event.Email?.IsVerified != true)
    {
      EmailVerifiedBy = null;
      EmailVerifiedOn = null;
    }
  }

  public void SetPassword(UserPasswordEvent @event)
  {
    Update(@event);

    PasswordHash = @event.Password.Encode();
    PasswordChangedBy = @event.ActorId.Value;
    PasswordChangedOn = @event.OccurredOn.ToUniversalTime();
  }

  public void SetPhone(UserPhoneChangedEvent @event)
  {
    Update(@event);

    PhoneCountryCode = @event.Phone?.CountryCode;
    PhoneNumber = @event.Phone?.Number;
    PhoneExtension = @event.Phone?.Extension;
    PhoneE164Formatted = @event.Phone?.FormatToE164();

    if (!IsPhoneVerified && @event.Phone?.IsVerified == true)
    {
      PhoneVerifiedBy = @event.ActorId.Value;
      PhoneVerifiedOn = @event.OccurredOn.ToUniversalTime();
    }
    else if (IsPhoneVerified && @event.Phone?.IsVerified != true)
    {
      PhoneVerifiedBy = null;
      PhoneVerifiedOn = null;
    }
  }

  public void SetUniqueName(UserUniqueNameChangedEvent @event)
  {
    Update(@event);

    UniqueName = @event.UniqueName.Value;
  }

  public void SignIn(UserSignedInEvent @event)
  {
    Update(@event);

    AuthenticatedOn = @event.OccurredOn;
  }

  public void Update(UserUpdatedEvent @event)
  {
    base.Update(@event);

    if (@event.FirstName != null)
    {
      FirstName = @event.FirstName.Value?.Value;
    }
    if (@event.MiddleName != null)
    {
      MiddleName = @event.MiddleName.Value?.Value;
    }
    if (@event.LastName != null)
    {
      LastName = @event.LastName.Value?.Value;
    }
    if (@event.FullName != null)
    {
      FullName = @event.FullName.Value;
    }
    if (@event.Nickname != null)
    {
      Nickname = @event.Nickname.Value?.Value;
    }

    if (@event.Birthdate != null)
    {
      Birthdate = @event.Birthdate.Value?.ToUniversalTime();
    }
    if (@event.Gender != null)
    {
      Gender = @event.Gender.Value?.Value;
    }
    if (@event.Locale != null)
    {
      Locale = @event.Locale.Value?.Code;
    }
    if (@event.TimeZone != null)
    {
      TimeZone = @event.TimeZone.Value?.Id;
    }

    if (@event.Picture != null)
    {
      Picture = @event.Picture.Value?.Value;
    }
    if (@event.Profile != null)
    {
      Profile = @event.Profile.Value?.Value;
    }
    if (@event.Website != null)
    {
      Website = @event.Website.Value?.Value;
    }

    foreach (KeyValuePair<string, string?> customAttribute in @event.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        CustomAttributes.Remove(customAttribute.Key);
      }
      else
      {
        CustomAttributes[customAttribute.Key] = customAttribute.Value;
      }
    }
  }
}
