﻿using Logitar.EventSourcing;
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

  public bool IsConfirmed
  {
    get => IsEmailVerified;
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

  public List<SessionEntity> Sessions { get; private set; } = [];

  public UserEntity(UserCreatedEvent @event) : base(@event)
  {
    TenantId = @event.TenantId?.Value;

    UniqueName = @event.UniqueName.Value;
  }

  private UserEntity() : base()
  {
  }

  public override IEnumerable<ActorId> GetActorIds() => GetActorIds(skipSessions: false);
  public IEnumerable<ActorId> GetActorIds(bool skipSessions)
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

    if (EmailVerifiedBy != null)
    {
      actorIds.Add(new ActorId(EmailVerifiedBy));
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
