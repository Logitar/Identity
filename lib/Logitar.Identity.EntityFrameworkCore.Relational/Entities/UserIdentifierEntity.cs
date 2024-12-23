using Logitar.Identity.Core.Users.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public sealed class UserIdentifierEntity : IdentifierEntity
{
  public int UserIdentifierId { get; private set; }

  public UserEntity? User { get; private set; }
  public int UserId { get; private set; }

  public UserIdentifierEntity(UserEntity user, UserIdentifierChanged @event) : base()
  {
    TenantId = user.TenantId;

    User = user;
    UserId = user.UserId;

    Key = @event.Key.Value;

    Update(@event);
  }

  private UserIdentifierEntity() : base()
  {
  }

  public void Update(UserIdentifierChanged @event)
  {
    Value = @event.Value.Value;
  }

  public override bool Equals(object? obj) => obj is UserIdentifierEntity identifier && identifier.UserIdentifierId == UserIdentifierId;
  public override int GetHashCode() => UserIdentifierId.GetHashCode();
  public override string ToString() => $"{GetType()} (UserIdentifierId={UserIdentifierId})";
}
