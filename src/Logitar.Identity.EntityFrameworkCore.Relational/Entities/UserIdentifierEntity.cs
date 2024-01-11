using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class UserIdentifierEntity : IdentifierEntity
{
  public int UserIdentifierId { get; private set; }

  public UserEntity? User { get; private set; }
  public int UserId { get; private set; }

  public UserIdentifierEntity(UserEntity user, UserIdentifierChangedEvent @event) : base()
  {
    TenantId = user.TenantId;

    User = user;
    UserId = user.UserId;

    Key = @event.Key;

    Update(@event);
  }

  private UserIdentifierEntity() : base()
  {
  }

  public void Update(UserIdentifierChangedEvent @event)
  {
    Value = @event.Value;
  }
}
