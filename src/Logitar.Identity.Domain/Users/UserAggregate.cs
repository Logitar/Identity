﻿using Logitar.EventSourcing;
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

  public EmailUnit? Email { get; private set; }
  public bool IsConfirmed => Email?.IsVerified == true;

  private PersonNameUnit? _firstName = null;
  public PersonNameUnit? FirstName
  {
    get => _firstName;
    private set
    {
      if (value != _firstName)
      {
        _firstName = value;
        FullName = PersonHelper.BuildFullName(_firstName, _middleName, _lastName);

        _updatedEvent.FirstName = new Modification<PersonNameUnit>(value);
        _updatedEvent.FullName = new Modification<string>(FullName);
      }
    }
  }
  private PersonNameUnit? _middleName = null;
  public PersonNameUnit? MiddleName
  {
    get => _middleName;
    private set
    {
      if (value != _middleName)
      {
        _middleName = value;
        FullName = PersonHelper.BuildFullName(_firstName, _middleName, _lastName);

        _updatedEvent.MiddleName = new Modification<PersonNameUnit>(value);
        _updatedEvent.FullName = new Modification<string>(FullName);
      }
    }
  }
  private PersonNameUnit? _lastName = null;
  public PersonNameUnit? LastName
  {
    get => _lastName;
    private set
    {
      if (value != _lastName)
      {
        _lastName = value;
        FullName = PersonHelper.BuildFullName(_firstName, _middleName, _lastName);

        _updatedEvent.LastName = new Modification<PersonNameUnit>(value);
        _updatedEvent.FullName = new Modification<string>(FullName);
      }
    }
  }
  public string? FullName { get; private set; }
  private PersonNameUnit? _nickname = null;
  public PersonNameUnit? Nickname
  {
    get => _nickname;
    private set
    {
      if (value != _nickname)
      {
        _nickname = value;
        _updatedEvent.Nickname = new Modification<PersonNameUnit>(value);
      }
    }
  }

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
  protected virtual void Apply(UserUpdatedEvent @event)
  {
    if (@event.FirstName != null)
    {
      _firstName = @event.FirstName.Value;
    }
    if (@event.MiddleName != null)
    {
      _middleName = @event.MiddleName.Value;
    }
    if (@event.LastName != null)
    {
      _lastName = @event.LastName.Value;
    }
    if (@event.FullName != null)
    {
      FullName = @event.FullName.Value;
    }
    if (@event.Nickname != null)
    {
      _nickname = @event.Nickname.Value;
    }
  }

  public override string ToString() => $"{FullName ?? UniqueName.Value} | {base.ToString()}";
}
