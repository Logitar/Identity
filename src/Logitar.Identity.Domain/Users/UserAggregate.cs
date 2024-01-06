using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.Domain.Users.Validators;

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

  public bool IsDisabled { get; private set; }

  // TODO(fpion): Address
  public EmailUnit? Email { get; private set; }
  // TODO(fpion): Phone
  public bool IsConfirmed => Email?.IsVerified == true;

  private PersonNameUnit? _firstName = null;
  public PersonNameUnit? FirstName
  {
    get => _firstName;
    set
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
    set
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
    set
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
    set
    {
      if (value != _nickname)
      {
        _nickname = value;
        _updatedEvent.Nickname = new Modification<PersonNameUnit>(value);
      }
    }
  }

  private DateTime? _birthdate = null;
  public DateTime? Birthdate
  {
    get => _birthdate;
    set
    {
      if (value != _birthdate)
      {
        if (value.HasValue)
        {
          new BirthdateValidator().ValidateAndThrow(value.Value);
        }

        _birthdate = value;
        _updatedEvent.Birthdate = new Modification<DateTime?>(value);
      }
    }
  }
  private GenderUnit? _gender = null;
  public GenderUnit? Gender
  {
    get => _gender;
    set
    {
      if (value != _gender)
      {
        _gender = value;
        _updatedEvent.Gender = new Modification<GenderUnit>(value);
      }
    }
  }
  private LocaleUnit? _locale = null;
  public LocaleUnit? Locale
  {
    get => _locale;
    set
    {
      if (value != _locale)
      {
        _locale = value;
        _updatedEvent.Locale = new Modification<LocaleUnit>(value);
      }
    }
  }
  private TimeZoneUnit? _timeZone = null;
  public TimeZoneUnit? TimeZone
  {
    get => _timeZone;
    set
    {
      if (value != _timeZone)
      {
        _timeZone = value;
        _updatedEvent.TimeZone = new Modification<TimeZoneUnit>(value);
      }
    }
  }

  private UrlUnit? _picture = null;
  public UrlUnit? Picture
  {
    get => _picture;
    set
    {
      if (value != _picture)
      {
        _picture = value;
        _updatedEvent.Picture = new Modification<UrlUnit>(value);
      }
    }
  }
  private UrlUnit? _profile = null;
  public UrlUnit? Profile
  {
    get => _profile;
    set
    {
      if (value != _profile)
      {
        _profile = value;
        _updatedEvent.Profile = new Modification<UrlUnit>(value);
      }
    }
  }
  private UrlUnit? _website = null;
  public UrlUnit? Website
  {
    get => _website;
    set
    {
      if (value != _website)
      {
        _website = value;
        _updatedEvent.Website = new Modification<UrlUnit>(value);
      }
    }
  }

  public DateTime? AuthenticatedOn { get; private set; }

  // TODO(fpion): Custom Attributes

  // TODO(fpion): Roles

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
    if (IsDeleted)
    {
      throw new UserIsDisabledException(this);
    }
    else if (password != null)
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

    if (@event.Birthdate != null)
    {
      _birthdate = @event.Birthdate.Value;
    }
    if (@event.Gender != null)
    {
      _gender = @event.Gender.Value;
    }
    if (@event.Locale != null)
    {
      _locale = @event.Locale.Value;
    }
    if (@event.TimeZone != null)
    {
      _timeZone = @event.TimeZone.Value;
    }

    if (@event.Picture != null)
    {
      _picture = @event.Picture.Value;
    }
    if (@event.Profile != null)
    {
      _profile = @event.Profile.Value;
    }
    if (@event.Website != null)
    {
      _website = @event.Website.Value;
    }
  }

  public override string ToString() => $"{FullName ?? UniqueName.Value} | {base.ToString()}";
}
