using Logitar.Identity.Domain.Roles.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class RoleEntity : AggregateEntity
{
  public int RoleId { get; private set; }

  public string? TenantId { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => UniqueName.ToUpper();
    private set { }
  }

  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

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

  public List<UserEntity> Users { get; private set; } = [];

  public RoleEntity(RoleCreatedEvent @event) : base(@event)
  {
    TenantId = @event.TenantId?.Value;

    UniqueName = @event.UniqueName.Value;
  }

  private RoleEntity() : base()
  {
  }

  public void SetUniqueName(RoleUniqueNameChangedEvent @event)
  {
    Update(@event);

    UniqueName = @event.UniqueName.Value;
  }

  public void Update(RoleUpdatedEvent @event)
  {
    base.Update(@event);

    if (@event.DisplayName != null)
    {
      DisplayName = @event.DisplayName.Value?.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
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
