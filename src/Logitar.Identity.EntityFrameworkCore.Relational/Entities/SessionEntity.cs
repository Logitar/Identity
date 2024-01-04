using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class SessionEntity : AggregateEntity
{
  public int SessionId { get; private set; }

  public UserEntity? User { get; private set; }
  public int UserId { get; private set; }

  public bool IsActive { get; private set; }

  public SessionEntity(UserEntity user, SessionCreatedEvent @event) : base(@event)
  {
    User = user;
    UserId = user.UserId;

    IsActive = true;
  }

  private SessionEntity() : base()
  {
  }

  public IEnumerable<ActorId> GetActorIds(bool ignoreUser = false)
  {
    List<ActorId> actorIds = [];
    actorIds.AddRange(base.GetActorIds());

    if (!ignoreUser && User != null)
    {
      actorIds.AddRange(User.GetActorIds(ignoreSessions: true));
    }

    return actorIds.AsReadOnly();
  }
}
