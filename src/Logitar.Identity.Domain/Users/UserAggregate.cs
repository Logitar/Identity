using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// TODO(fpion): document
/// </summary>
public class UserAggregate : AggregateRoot
{
  private readonly Dictionary<string, string> _customAttributes = new();
  private UserUpdatedEvent _updated = new();

  /// <summary>
  /// Gets the identifier of the user.
  /// </summary>
  public new UserId Id => new(base.Id);

  /// <summary>
  /// Gets the tenant identifier of the user.
  /// </summary>
  public TenantId? TenantId { get; private set; }

  /// <summary>
  /// The unique name of the user.
  /// </summary>
  private UniqueNameUnit? _uniqueName = null;
  /// <summary>
  /// Gets the unique name of the user.
  /// </summary>
  public UniqueNameUnit UniqueName => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");

  /// <summary>
  /// Gets or sets a value indicating whether or not the user is disabled.
  /// </summary>
  public bool IsDisabled { get; private set; }

  private PersonNameUnit? _firstName = null;
  /// <summary>
  /// Gets or sets the first name of the user.
  /// </summary>
  public PersonNameUnit? FirstName
  {
    get => _firstName;
    set
    {
      if (value != _firstName)
      {
        _updated.FirstName = new Modification<PersonNameUnit>(value);
        _updated.FullName = new Modification<string>(BuildFullName(value, MiddleName, LastName));

        _firstName = value;
        FullName = _updated.FullName.Value;
      }
    }
  }
  private PersonNameUnit? _middleName = null;
  /// <summary>
  /// Gets or sets the middle name of the user.
  /// </summary>
  public PersonNameUnit? MiddleName
  {
    get => _middleName;
    set
    {
      if (value != _middleName)
      {
        _updated.MiddleName = new Modification<PersonNameUnit>(value);
        _updated.FullName = new Modification<string>(BuildFullName(FirstName, value, LastName));

        _middleName = value;
        FullName = _updated.FullName.Value;
      }
    }
  }
  private PersonNameUnit? _lastName = null;
  /// <summary>
  /// Gets or sets the last name of the user.
  /// </summary>
  public PersonNameUnit? LastName
  {
    get => _lastName;
    set
    {
      if (value != _lastName)
      {
        _updated.LastName = new Modification<PersonNameUnit>(value);
        _updated.FullName = new Modification<string>(BuildFullName(FirstName, MiddleName, value));

        _lastName = value;
        FullName = _updated.FullName.Value;
      }
    }
  }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public string? FullName { get; private set; }
  private PersonNameUnit? _nickname = null;
  /// <summary>
  /// Gets or sets the nickname of the user.
  /// </summary>
  public PersonNameUnit? Nickname
  {
    get => _nickname;
    set
    {
      if (value != _nickname)
      {
        _updated.Nickname = new Modification<PersonNameUnit>(value);
        _nickname = value;
      }
    }
  }

  /// <summary>
  /// Gets the custom attributes of the user.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAggregate"/> class.
  /// DO NOT use this constructor to create a new user. It is only intended to be used by the event sourcing.
  /// </summary>
  /// <param name="id">The identifier of the user.</param>
  public UserAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAggregate"/> class.
  /// DO use this constructor to create a new user.
  /// </summary>
  /// <param name="uniqueName">The unique name of the user.</param>
  /// <param name="tenantId">The tenant identifier of the user.</param>
  /// <param name="actorId">The actor identifier.</param>
  /// <param name="id">The identifier of the user.</param>
  public UserAggregate(UniqueNameUnit uniqueName, TenantId? tenantId = null, ActorId actorId = default, UserId? id = null)
    : base(id?.AggregateId)
  {
    ApplyChange(new UserCreatedEvent(actorId, uniqueName, tenantId));
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserCreatedEvent @event)
  {
    TenantId = @event.TenantId;

    _uniqueName = @event.UniqueName;
  }

  /// <summary>
  /// Deletes the user, if it is not already deleted.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      ApplyChange(new UserDeletedEvent(actorId));
    }
  }

  /// <summary>
  /// Disables the user, if it is enabled.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Disable(ActorId actorId = default)
  {
    if (!IsDisabled)
    {
      ApplyChange(new UserDisabledEvent(actorId));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Apply(UserDisabledEvent _) => IsDisabled = true;

  /// <summary>
  /// Enables the user, if it is disabled.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Enable(ActorId actorId = default)
  {
    if (IsDisabled)
    {
      ApplyChange(new UserEnabledEvent(actorId));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="_">The event to apply.</param>
  protected virtual void Apply(UserEnabledEvent _) => IsDisabled = false;

  /// <summary>
  /// Removes the specified custom attribute on the user.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();

    if (_customAttributes.ContainsKey(key))
    {
      _updated.CustomAttributes[key] = null;
      _customAttributes.Remove(key);
    }
  }

  private readonly CustomAttributeValidator _customAttributeValidator = new();
  /// <summary>
  /// Sets the specified custom attribute on the user.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    _customAttributeValidator.ValidateAndThrow(key, value);

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || existingValue != value)
    {
      _updated.CustomAttributes[key] = value;
      _customAttributes[key] = value;
    }
  }

  /// <summary>
  /// Sets the unique name of the user.
  /// </summary>
  /// <param name="uniqueName">The unique name.</param>
  /// <param name="actorId">The actor identifier.</param>
  public void SetUniqueName(UniqueNameUnit uniqueName, ActorId actorId = default)
  {
    if (uniqueName != _uniqueName)
    {
      ApplyChange(new UserUniqueNameChangedEvent(actorId, uniqueName));
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
  protected virtual void Apply(UserUniqueNameChangedEvent @event) => _uniqueName = @event.UniqueName;

  /// <summary>
  /// Applies updates on the user.
  /// </summary>
  /// <param name="actorId">The actor identifier.</param>
  public void Update(ActorId actorId = default)
  {
    if (_updated.HasChanges)
    {
      _updated.ActorId = actorId;

      ApplyChange(_updated);

      _updated = new();
    }
  }
  /// <summary>
  /// Applies the specified event.
  /// </summary>
  /// <param name="event">The event to apply.</param>
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

  /// <summary>
  /// Returns a string representation of the user.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{FullName ?? UniqueName.Value} | {base.ToString()}";

  private static string? BuildFullName(params PersonNameUnit?[] names) => string.Join(' ', names
    .SelectMany(name => name?.Value.Split() ?? Array.Empty<string>())
    .Where(name => !string.IsNullOrEmpty(name))).CleanTrim();
}
