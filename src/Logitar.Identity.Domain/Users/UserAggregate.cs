using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

public class UserAggregate : AggregateRoot
{
  private UserUpdatedEvent _updatedEvent = new();

  public new UserId Id => new(base.Id);

  public TenantId? TenantId { get; private set; }

  private UniqueNameUnit? _uniqueName = null;
  public UniqueNameUnit UniqueName => _uniqueName ?? throw new InvalidOperationException("The unique name has not been initialized yet.");

  public bool IsDisabled { get; private set; }

  public EmailUnit? Email { get; private set; }

  public bool IsConfirmed => Email?.IsVerified == true;

  public PersonNameUnit? _firstName = null;
  public PersonNameUnit? FirstName
  {
    get => _firstName;
    set
    {
      if (value != _firstName)
      {
        _firstName = value;
        _updatedEvent.FirstName = new Modification<PersonNameUnit>(value);
        _updatedEvent.FullName = new Modification<string>(UserHelper.BuildFullName(_firstName, _middleName, _lastName));
      }
    }
  }
  public PersonNameUnit? _middleName = null;
  public PersonNameUnit? MiddleName
  {
    get => _middleName;
    set
    {
      if (value != _middleName)
      {
        _middleName = value;
        _updatedEvent.MiddleName = new Modification<PersonNameUnit>(value);
        _updatedEvent.FullName = new Modification<string>(UserHelper.BuildFullName(_firstName, _middleName, _lastName));
      }
    }
  }
  public PersonNameUnit? _lastName = null;
  public PersonNameUnit? LastName
  {
    get => _lastName;
    set
    {
      if (value != _lastName)
      {
        _lastName = value;
        _updatedEvent.LastName = new Modification<PersonNameUnit>(value);
        _updatedEvent.FullName = new Modification<string>(UserHelper.BuildFullName(_firstName, _middleName, _lastName));
      }
    }
  }
  public string? FullName { get; private set; }
  public PersonNameUnit? _nickname = null;
  public PersonNameUnit? Nickname
  {
    get => _nickname;
    set
    {
      if (value != _nickname)
      {
        _nickname = value;
        _updatedEvent.Nickname = new Modification<PersonNameUnit>(value);
      }
    }
  }

  public UserAggregate(AggregateId id) : base(id)
  {
  }

  public UserAggregate(UniqueNameUnit uniqueName, TenantId? tenantId = null, ActorId actorId = default, UserId? id = null)
    : base((id ?? UserId.NewId())?.AggregateId)
  {
    Raise(new UserCreatedEvent(actorId, tenantId, uniqueName));
  }
  protected virtual void Apply(UserCreatedEvent @event)
  {
    TenantId = @event.TenantId;

    _uniqueName = @event.UniqueName;
  }

  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new UserDeletedEvent(actorId));
    }
  }

  public void Disable(ActorId actorId = default)
  {
    if (!IsDisabled)
    {
      Raise(new UserDisabledEvent(actorId));
    }
  }
  protected virtual void Apply(UserDisabledEvent _)
  {
    IsDisabled = true;
  }

  public void Enable(ActorId actorId = default)
  {
    if (IsDisabled)
    {
      Raise(new UserEnabledEvent(actorId));
    }
  }
  protected virtual void Apply(UserEnabledEvent _)
  {
    IsDisabled = false;
  }

  public void SetEmail(EmailUnit? email, ActorId actorId = default)
  {
    if (email != Email)
    {
      Raise(new UserEmailChangedEvent(actorId, email));
    }
  }
  protected virtual void Apply(UserEmailChangedEvent @event)
  {
    Email = @event.Email;
  }

  public void SetUniqueName(UniqueNameUnit uniqueName, ActorId actorId = default)
  {
    if (uniqueName != UniqueName)
    {
      Raise(new UserUniqueNameChangedEvent(actorId, uniqueName));
    }
  }
  protected virtual void Apply(UserUniqueNameChangedEvent @event)
  {
    _uniqueName = @event.UniqueName;
  }

  public void Update(ActorId actorId = default)
  {
    if (_updatedEvent.HasChanges)
    {
      _updatedEvent.ActorId = actorId;
      Raise(_updatedEvent);
      _updatedEvent = new();
    }
  }
  protected virtual void Apply(UserUpdatedEvent @event)
  {
    if (@event.FirstName != null)
    {
      _firstName = @event.FirstName?.Value;
    }
    if (@event.MiddleName != null)
    {
      _middleName = @event.MiddleName?.Value;
    }
    if (@event.LastName != null)
    {
      _lastName = @event.LastName?.Value;
    }
    if (@event.FullName != null)
    {
      FullName = @event.FullName?.Value;
    }
    if (@event.Nickname != null)
    {
      _nickname = @event.Nickname?.Value;
    }
  }

  public override string ToString() => $"{FullName ?? UniqueName.Value} | {base.ToString()}";
}
