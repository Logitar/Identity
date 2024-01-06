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

  public string? FullName { get; private set; }

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

    if (!skipSessions)
    {
      foreach (SessionEntity session in Sessions)
      {
        actorIds.AddRange(session.GetActorIds(skipUser: true));
      }
    }

    return actorIds.AsReadOnly();
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
  }
}
