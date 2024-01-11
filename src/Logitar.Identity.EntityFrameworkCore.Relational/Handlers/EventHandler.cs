using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public abstract class EventHandler
{
  protected IdentityContext Context { get; }

  protected EventHandler(IdentityContext context)
  {
    Context = context;
  }

  protected async Task SynchronizeCustomAttributesAsync(string entityType, int entityId, Dictionary<string, string?> customAttributes, CancellationToken cancellationToken = default)
  {
    if (customAttributes.Count == 0)
    {
      return;
    }

    Dictionary<string, CustomAttributeEntity> entities = (await Context.CustomAttributes
      .Where(x => x.EntityType == entityType && x.EntityId == entityId)
      .ToArrayAsync(cancellationToken)
    ).ToDictionary(x => x.Key, x => x);

    foreach (KeyValuePair<string, string?> customAttribute in customAttributes)
    {
      if (customAttribute.Value == null)
      {
        if (entities.TryGetValue(customAttribute.Key, out CustomAttributeEntity? entity))
        {
          Context.CustomAttributes.Remove(entity);
        }
      }
      else
      {
        if (!entities.TryGetValue(customAttribute.Key, out CustomAttributeEntity? entity))
        {
          entity = new()
          {
            EntityType = entityType,
            EntityId = entityId,
            Key = customAttribute.Key
          };

          Context.CustomAttributes.Add(entity);
          entities[customAttribute.Key] = entity;
        }

        entity.Value = customAttribute.Value;
      }
    }
  }
}
