﻿using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Sessions;

public class SessionAggregate : AggregateRoot
{
  public new SessionId Id => new(base.Id);

  private UserId? _userId = null;
  public UserId UserId => _userId ?? throw new InvalidOperationException($"The {nameof(UserId)} has not been initialized yet.");

  private Password? _secret = null;
  public bool IsPersistent => _secret != null;

  public bool IsActive { get; private set; }

  public SessionAggregate(AggregateId id) : base(id)
  {
  }

  public SessionAggregate(UserAggregate user, Password? secret = null, ActorId actorId = default, SessionId? id = null)
    : base((id ?? SessionId.NewId()).AggregateId)
  {
    Raise(new SessionCreatedEvent(actorId, secret, user.Id));
  }
  protected virtual void Apply(SessionCreatedEvent @event)
  {
    _userId = @event.UserId;

    _secret = @event.Secret;

    IsActive = true;
  }

  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new SessionDeletedEvent(actorId));
    }
  }
}