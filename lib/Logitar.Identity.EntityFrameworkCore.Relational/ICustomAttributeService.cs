using Logitar.Identity.Core;

namespace Logitar.Identity.EntityFrameworkCore.Relational;


public interface ICustomAttributeService
{
  Task RemoveAsync(string entityType, int entityId, CancellationToken cancellationToken = default);
  Task UpdateAsync(string entityType, int entityId, IReadOnlyDictionary<Identifier, string?> customAttributes, CancellationToken cancellationToken = default);
}
