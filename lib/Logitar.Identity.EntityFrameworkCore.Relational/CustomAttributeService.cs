using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public class CustomAttributeService : ICustomAttributeService
{
  protected virtual IdentityContext Context { get; }

  public CustomAttributeService(IdentityContext context)
  {
    Context = context;
  }

  public virtual async Task RemoveAsync(string entityType, int entityId, CancellationToken cancellationToken)
  {
    CustomAttributeEntity[] entities = await Context.CustomAttributes
      .Where(x => x.EntityType == entityType && x.EntityId == entityId)
      .ToArrayAsync(cancellationToken);
    Context.CustomAttributes.RemoveRange(entities);
  }

  public virtual async Task UpdateAsync(string entityType, int entityId, IReadOnlyDictionary<Identifier, string?> customAttributes, CancellationToken cancellationToken)
  {
    if (customAttributes.Count < 1)
    {
      return;
    }

    Dictionary<string, CustomAttributeEntity> entities = (await Context.CustomAttributes
      .Where(x => x.EntityType == entityType && x.EntityId == entityId)
      .ToArrayAsync(cancellationToken)).ToDictionary(x => x.Key, x => x);

    foreach (KeyValuePair<Identifier, string?> customAttribute in customAttributes)
    {
      if (customAttribute.Value == null)
      {
        if (entities.TryGetValue(customAttribute.Key.Value, out CustomAttributeEntity? entity))
        {
          Context.CustomAttributes.Remove(entity);
        }
      }
      else
      {
        if (!entities.TryGetValue(customAttribute.Key.Value, out CustomAttributeEntity? entity))
        {
          entity = new()
          {
            EntityType = entityType,
            EntityId = entityId,
            Key = customAttribute.Key.Value
          };

          Context.CustomAttributes.Add(entity);
          entities[customAttribute.Key.Value] = entity;
        }

        entity.Value = customAttribute.Value;
      }
    }
  }
}
