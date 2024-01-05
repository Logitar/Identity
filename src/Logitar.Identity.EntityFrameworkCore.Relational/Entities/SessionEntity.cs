using Logitar.Identity.Domain.Sessions.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class SessionEntity : AggregateEntity
{
  public int SessionId { get; private set; }

  public UserEntity? User { get; private set; }
  public int UserId { get; private set; }

  public bool IsActive { get; private set; }

  public SessionEntity(UserEntity user, SessionCreatedEvent e) : base(e)
  {
    User = user;
    UserId = user.UserId;

    IsActive = true;
  }

  private SessionEntity() : base()
  {
  }
}
