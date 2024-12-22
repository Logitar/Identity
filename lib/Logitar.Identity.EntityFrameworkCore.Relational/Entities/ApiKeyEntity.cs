using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.ApiKeys.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public sealed class ApiKeyEntity : AggregateEntity
{
  public int ApiKeyId { get; private set; }

  public string? TenantId { get; private set; }
  public string EntityId { get; private set; } = string.Empty;

  public string SecretHash { get; private set; } = string.Empty;

  public string DisplayName { get; private set; } = string.Empty;
  public string? Description { get; private set; }
  public DateTime? ExpiresOn { get; private set; }

  public DateTime? AuthenticatedOn { get; private set; }

  public string? CustomAttributes { get; private set; }

  public List<RoleEntity> Roles { get; private set; } = [];

  public ApiKeyEntity(ApiKeyCreated @event) : base(@event)
  {
    ApiKeyId apiKeyId = new(@event.StreamId);
    TenantId = apiKeyId.TenantId?.Value;
    EntityId = apiKeyId.EntityId.Value;

    SecretHash = @event.Secret.Encode();

    DisplayName = @event.DisplayName.Value;
  }

  private ApiKeyEntity() : base()
  {
  }

  public override IReadOnlyCollection<ActorId> GetActorIds() => GetActorIds(skipRoles: false);
  public IReadOnlyCollection<ActorId> GetActorIds(bool skipRoles)
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

  public void AddRole(RoleEntity role, ApiKeyRoleAdded @event)
  {
    Update(@event);

    Roles.Add(role);
  }

  public void Authenticate(ApiKeyAuthenticated @event)
  {
    Update(@event);

    AuthenticatedOn = @event.OccurredOn.ToUniversalTime();
  }

  public void RemoveRole(ApiKeyRoleRemoved @event)
  {
    Update(@event);

    RoleEntity? role = Roles.SingleOrDefault(x => x.StreamId == @event.RoleId.StreamId.Value);
    if (role != null)
    {
      Roles.Remove(role);
    }
  }

  public void Update(ApiKeyUpdated @event)
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

    Dictionary<string, string> customAttributes = GetCustomAttributes();
    foreach (KeyValuePair<Identifier, string?> customAttribute in @event.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        customAttributes.Remove(customAttribute.Key.Value);
      }
      else
      {
        customAttributes[customAttribute.Key.Value] = customAttribute.Value;
      }
    }
    SetCustomAttributes(customAttributes);
  }

  public Dictionary<string, string> GetCustomAttributes()
  {
    return (CustomAttributes == null ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(CustomAttributes)) ?? [];
  }
  private void SetCustomAttributes(Dictionary<string, string> customAttributes)
  {
    CustomAttributes = customAttributes.Count < 1 ? null : JsonSerializer.Serialize(customAttributes);
  }

  public override string ToString() => $"{DisplayName} | {base.ToString()}";
}
