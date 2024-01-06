using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

public class UserAggregate : AggregateRoot
{
  private UserUpdatedEvent _updatedEvent = new();

  public new UserId Id => new(base.Id);

  public TenantId? TenantId { get; private set; }

  private UniqueNameUnit? _uniqueName = null;
  public UniqueNameUnit UniqueName => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");

  private Password? _password = null;
  public bool HasPassword => _password != null;

  public string? FullName { get; private set; }

  public DateTime? AuthenticatedOn { get; private set; }

  public UserAggregate(AggregateId id) : base(id)
  {
  }

  public UserAggregate(UniqueNameUnit uniqueName, TenantId? tenantId = null, ActorId actorId = default, UserId? id = null)
    : base((id ?? UserId.NewId()).AggregateId)
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

  public void SetPassword(Password password, ActorId actorId = default)
  {
    Raise(new UserPasswordChangedEvent(actorId, password));
  }
  protected virtual void Apply(UserPasswordChangedEvent @event)
  {
    _password = @event.Password;
  }

  public SessionAggregate SignIn(string? password = null, Password? secret = null, ActorId actorId = default, SessionId? id = null)
  {
    if (password != null)
    {
      if (_password == null)
      {
        throw new UserHasNoPasswordException(this);
      }
      else if (!_password.IsMatch(password))
      {
        throw new IncorrectUserPasswordException(this, password);
      }
    }

    SessionAggregate session = new(this, secret, actorId, id);

    Raise(new UserSignedInEvent(actorId, session.CreatedOn));

    return session;
  }
  protected virtual void Apply(UserSignedInEvent @event)
  {
    AuthenticatedOn = @event.OccurredOn;
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

  public override string ToString() => $"{FullName ?? UniqueName.Value} | {base.ToString()}";
}
