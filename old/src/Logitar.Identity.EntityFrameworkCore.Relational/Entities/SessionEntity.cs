using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class SessionEntity : AggregateEntity
{
  public int SessionId { get; private set; }

  public UserEntity? User { get; private set; }
  public int UserId { get; private set; }

  public string? SecretHash { get; private set; }
  public bool IsPersistent
  {
    get => SecretHash != null;
    private set { }
  }

  public string? SignedOutBy { get; private set; }
  public DateTime? SignedOutOn { get; private set; }
  public bool IsActive
  {
    get => SignedOutBy == null && SignedOutOn == null;
    private set { }
  }

  public Dictionary<string, string> CustomAttributes { get; private set; } = [];
  public string? CustomAttributesSerialized
  {
    get => CustomAttributes.Count == 0 ? null : JsonSerializer.Serialize(CustomAttributes);
    private set
    {
      if (value == null)
      {
        CustomAttributes.Clear();
      }
      else
      {
        CustomAttributes = JsonSerializer.Deserialize<Dictionary<string, string>>(value) ?? [];
      }
    }
  }

  public SessionEntity(UserEntity user, SessionCreatedEvent @event) : base(@event)
  {
    User = user;
    UserId = user.UserId;

    SecretHash = @event.Secret?.Encode();
  }

  private SessionEntity() : base()
  {
  }

  public override IEnumerable<ActorId> GetActorIds() => GetActorIds(skipUser: false);
  public IEnumerable<ActorId> GetActorIds(bool skipUser)
  {
    List<ActorId> actorIds = [];
    actorIds.AddRange(base.GetActorIds());

    if (SignedOutBy != null)
    {
      actorIds.Add(new ActorId(SignedOutBy));
    }

    if (!skipUser && User != null)
    {
      actorIds.AddRange(User.GetActorIds(skipRoles: false, skipSessions: true));
    }

    return actorIds.AsReadOnly();
  }

  public void Renew(SessionRenewedEvent @event)
  {
    Update(@event);

    SecretHash = @event.Secret.Encode();
  }

  public void SignOut(SessionSignedOutEvent @event)
  {
    Update(@event);

    SignedOutBy = @event.ActorId.Value;
    SignedOutOn = @event.OccurredOn.ToUniversalTime();
  }

  public void Update(SessionUpdatedEvent @event)
  {
    base.Update(@event);

    foreach (KeyValuePair<string, string?> customAttribute in @event.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        CustomAttributes.Remove(customAttribute.Key);
      }
      else
      {
        CustomAttributes[customAttribute.Key] = customAttribute.Value;
      }
    }
  }
}
