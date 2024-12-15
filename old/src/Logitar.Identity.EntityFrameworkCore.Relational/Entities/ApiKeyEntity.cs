using Logitar.EventSourcing;
using Logitar.Identity.Domain.ApiKeys.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class ApiKeyEntity : AggregateEntity
{
  public int ApiKeyId { get; private set; }

  public string? TenantId { get; private set; }

  public string SecretHash { get; private set; } = string.Empty;

  public string DisplayName { get; private set; } = string.Empty;
  public string? Description { get; private set; }
  public DateTime? ExpiresOn { get; private set; }

  public DateTime? AuthenticatedOn { get; private set; }

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

  public List<RoleEntity> Roles { get; private set; } = [];

  public ApiKeyEntity(ApiKeyCreatedEvent @event) : base(@event)
  {
    SecretHash = @event.Secret.Encode();

    TenantId = @event.TenantId?.Value;

    DisplayName = @event.DisplayName.Value;
  }

  private ApiKeyEntity() : base()
  {
  }

  public override IEnumerable<ActorId> GetActorIds() => GetActorIds(skipRoles: false);
  public IEnumerable<ActorId> GetActorIds(bool skipRoles)
  {
    List<ActorId> actorIds = [];
    actorIds.AddRange(base.GetActorIds());

    if (!skipRoles)
    {
      foreach (RoleEntity role in Roles)
      {
        actorIds.AddRange(role.GetActorIds());
      }
    }

    return actorIds.AsReadOnly();
  }

  public void AddRole(RoleEntity role, ApiKeyRoleAddedEvent @event)
  {
    Update(@event);

    Roles.Add(role);
  }

  public void Authenticate(ApiKeyAuthenticatedEvent @event)
  {
    Update(@event);

    AuthenticatedOn = @event.OccurredOn.ToUniversalTime();
  }

  public void RemoveRole(ApiKeyRoleRemovedEvent @event)
  {
    Update(@event);

    RoleEntity? role = Roles.SingleOrDefault(x => x.AggregateId == @event.RoleId.AggregateId.Value);
    if (role != null)
    {
      Roles.Remove(role);
    }
  }

  public void Update(ApiKeyUpdatedEvent @event)
  {
    base.Update(@event);

    if (@event.DisplayName != null)
    {
      DisplayName = @event.DisplayName.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
    }
    if (@event.ExpiresOn.HasValue)
    {
      ExpiresOn = @event.ExpiresOn.Value.ToUniversalTime();
    }

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
