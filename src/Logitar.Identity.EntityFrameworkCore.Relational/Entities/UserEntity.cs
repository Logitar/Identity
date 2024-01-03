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

  public void SetUniqueName(UserUniqueNameChangedEvent @event)
  {
    Update(@event);

    UniqueName = @event.UniqueName.Value;
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
  }
}
