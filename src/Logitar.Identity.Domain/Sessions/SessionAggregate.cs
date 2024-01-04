using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Sessions;

public class SessionAggregate : AggregateRoot
{
  public new SessionId Id => new(base.Id);

  private UserId? _userId = null;
  public UserId UserId => _userId ?? throw new InvalidOperationException("The user identifier has not been initialized yet.");

  public bool IsActive { get; private set; }

  public SessionAggregate(AggregateId id) : base(id)
  {
  }

  public SessionAggregate(UserAggregate user, ActorId actorId = default, SessionId? id = null)
    : base((id ?? SessionId.NewId()).AggregateId)
  {
    Raise(new SessionCreatedEvent(actorId, user.Id));
  }
  protected virtual void Apply(SessionCreatedEvent @event)
  {
    _userId = @event.UserId;

    IsActive = true;
  }
}
