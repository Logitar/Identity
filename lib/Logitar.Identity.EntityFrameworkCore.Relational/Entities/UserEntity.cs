using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Core.Users.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public sealed class UserEntity : AggregateEntity
{
  public int UserId { get; private set; }

  public string? TenantId { get; private set; }
  public string EntityId { get; private set; } = string.Empty;

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => IdentityDb.Helper.Normalize(UniqueName);
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
    get => EmailAddress == null ? null : IdentityDb.Helper.Normalize(EmailAddress);
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

  public string? CustomAttributes { get; private set; }

  public List<UserIdentifierEntity> Identifiers { get; private set; } = [];
  public List<RoleEntity> Roles { get; private set; } = [];
  public List<SessionEntity> Sessions { get; private set; } = [];

  public UserEntity(UserCreated @event) : base(@event)
  {
    UserId userId = new(@event.StreamId);
    TenantId = userId.TenantId?.Value;
    EntityId = userId.EntityId.Value;

    UniqueName = @event.UniqueName.Value;
  }

  private UserEntity() : base()
  {
  }

  public override IReadOnlyCollection<ActorId> GetActorIds() => GetActorIds(skipRoles: false, skipSessions: false);
  public IReadOnlyCollection<ActorId> GetActorIds(bool skipRoles, bool skipSessions)
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

  public void AddRole(RoleEntity role, UserRoleAdded @event)
  {
    Update(@event);

    Roles.Add(role);
  }

  public void Authenticate(UserAuthenticated @event)
  {
    Update(@event);

    AuthenticatedOn = @event.OccurredOn.ToUniversalTime();
  }

  public void Disable(UserDisabled @event)
  {
    Update(@event);

    DisabledBy = @event.ActorId?.Value;
    DisabledOn = @event.OccurredOn.ToUniversalTime();
  }

  public void Enable(UserEnabled @event)
  {
    Update(@event);

    DisabledBy = null;
    DisabledOn = null;
  }

  public void RemoveCustomIdentifier(UserIdentifierRemoved @event)
  {
    UserIdentifierEntity? identifier = Identifiers.SingleOrDefault(x => x.Key == @event.Key.Value);
    if (identifier != null)
    {
      Identifiers.Remove(identifier);
    }
  }

  public void RemoveRole(UserRoleRemoved @event)
  {
    Update(@event);

    RoleEntity? role = Roles.SingleOrDefault(x => x.StreamId == @event.RoleId.StreamId.Value);
    if (role != null)
    {
      Roles.Remove(role);
    }
  }

  public void SetAddress(UserAddressChanged @event)
  {
    Update(@event);

    AddressStreet = @event.Address?.Street;
    AddressLocality = @event.Address?.Locality;
    AddressPostalCode = @event.Address?.PostalCode;
    AddressRegion = @event.Address?.Region;
    AddressCountry = @event.Address?.Country;
    AddressFormatted = @event.Address?.ToString();

    if (!IsAddressVerified && @event.Address?.IsVerified == true)
    {
      AddressVerifiedBy = @event.ActorId?.Value;
      AddressVerifiedOn = @event.OccurredOn.ToUniversalTime();
    }
    else if (IsAddressVerified && @event.Address?.IsVerified != true)
    {
      AddressVerifiedBy = null;
      AddressVerifiedOn = null;
    }
  }

  public void SetCustomIdentifier(UserIdentifierChanged @event)
  {
    UserIdentifierEntity? identifier = Identifiers.SingleOrDefault(x => x.Key == @event.Key.Value);
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

  public void SetEmail(UserEmailChanged @event)
  {
    Update(@event);

    EmailAddress = @event.Email?.Address;

    if (!IsEmailVerified && @event.Email?.IsVerified == true)
    {
      EmailVerifiedBy = @event.ActorId?.Value;
      EmailVerifiedOn = @event.OccurredOn.ToUniversalTime();
    }
    else if (IsEmailVerified && @event.Email?.IsVerified != true)
    {
      EmailVerifiedBy = null;
      EmailVerifiedOn = null;
    }
  }

  public void RemovePassword(UserPasswordRemoved @event)
  {
    Update(@event);

    PasswordHash = null;
    PasswordChangedBy = null;
    PasswordChangedOn = null;
  }

  public void SetPassword(UserPasswordEvent @event)
  {
    Update(@event);

    PasswordHash = @event.Password.Encode();
    PasswordChangedBy = @event.ActorId?.Value;
    PasswordChangedOn = @event.OccurredOn.ToUniversalTime();
  }

  public void SetPhone(UserPhoneChanged @event)
  {
    Update(@event);

    PhoneCountryCode = @event.Phone?.CountryCode;
    PhoneNumber = @event.Phone?.Number;
    PhoneExtension = @event.Phone?.Extension;
    PhoneE164Formatted = @event.Phone?.FormatToE164();

    if (!IsPhoneVerified && @event.Phone?.IsVerified == true)
    {
      PhoneVerifiedBy = @event.ActorId?.Value;
      PhoneVerifiedOn = @event.OccurredOn.ToUniversalTime();
    }
    else if (IsPhoneVerified && @event.Phone?.IsVerified != true)
    {
      PhoneVerifiedBy = null;
      PhoneVerifiedOn = null;
    }
  }

  public void SetUniqueName(UserUniqueNameChanged @event)
  {
    Update(@event);

    UniqueName = @event.UniqueName.Value;
  }

  public void SignIn(UserSignedIn @event)
  {
    Update(@event);

    AuthenticatedOn = @event.OccurredOn.ToUniversalTime();
  }

  public void Update(UserUpdated @event)
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

    Dictionary<string, string> customAttributes = GetCustomAttributes();
    foreach (KeyValuePair<Identifier, string?> customAttribute in @event.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        customAttributes.Remove(customAttribute.Key.Value);
      }
      else
      {
        customAttributes[customAttribute.Key.Value] = customAttribute.Value;
      }
    }
    SetCustomAttributes(customAttributes);
  }

  public Dictionary<string, string> GetCustomAttributes()
  {
    return (CustomAttributes == null ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(CustomAttributes)) ?? [];
  }
  private void SetCustomAttributes(Dictionary<string, string> customAttributes)
  {
    CustomAttributes = customAttributes.Count < 1 ? null : JsonSerializer.Serialize(customAttributes);
  }

  public override string ToString() => $"{FullName ?? UniqueName} | {base.ToString()}";
}
