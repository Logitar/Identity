using Logitar.EventSourcing;
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
  // TODO(fpion): Locale
  public string? TimeZone { get; private set; }

  public DateTime? AuthenticatedOn { get; private set; }

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

  public void SetPassword(UserPasswordChangedEvent @event)
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
    // TODO(fpion): Locale
    if (@event.TimeZone != null)
    {
      TimeZone = @event.TimeZone.Value?.Id;
    }
  }
}
