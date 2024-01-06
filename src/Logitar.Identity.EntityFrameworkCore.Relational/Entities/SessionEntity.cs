using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class SessionEntity : AggregateEntity
{
  public int SessionId { get; private set; }

  public UserEntity? User { get; private set; }
  public int UserId { get; private set; }

  public string? SecretHash { get; private set; }
  public bool IsPersistent
  {
    get => SecretHash != null;
    private set { }
  }

  public bool IsActive { get; private set; }

  public SessionEntity(UserEntity user, SessionCreatedEvent @event) : base(@event)
  {
    User = user;
    UserId = user.UserId;

    SecretHash = @event.Secret?.Encode();

    IsActive = true;
  }

  private SessionEntity() : base()
  {
  }

  public override IEnumerable<ActorId> GetActorIds() => GetActorIds(skipUser: false);
  public IEnumerable<ActorId> GetActorIds(bool skipUser)
  {
    List<ActorId> actorIds = [];
    actorIds.AddRange(base.GetActorIds());

    if (!skipUser && User != null)
    {
      actorIds.AddRange(User.GetActorIds(skipSessions: true));
    }

    return actorIds.AsReadOnly();
  }
}
