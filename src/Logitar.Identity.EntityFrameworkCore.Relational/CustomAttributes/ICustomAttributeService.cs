namespace Logitar.Identity.EntityFrameworkCore.Relational.CustomAttributes;

public interface ICustomAttributeService
{
  Task RemoveAsync(string entityType, int entityId, CancellationToken cancellationToken = default);
  Task SynchronizeAsync(string entityType, int entityId, Dictionary<string, string?> customAttributes, CancellationToken cancellationToken = default);
}
