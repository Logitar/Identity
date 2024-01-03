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

  public string? DisabledBy { get; private set; }
  public DateTime? DisabledOn { get; private set; }
  public bool IsDisabled
  {
    get => DisabledBy != null && DisabledOn != null;
    private set { }
  }

  public string? FullName { get; private set; }

  public UserEntity(UserCreatedEvent @event) : base(@event)
  {
    TenantId = @event.TenantId?.Value;

    UniqueName = @event.UniqueName.Value;
  }

  private UserEntity() : base()
  {
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

  public void SetUniqueName(UserUniqueNameChangedEvent @event)
  {
    Update(@event);

    UniqueName = @event.UniqueName.Value;
  }
}
