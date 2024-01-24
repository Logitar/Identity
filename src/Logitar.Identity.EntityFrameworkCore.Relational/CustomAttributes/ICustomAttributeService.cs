namespace Logitar.Identity.EntityFrameworkCore.Relational.CustomAttributes;

public interface ICustomAttributeService
{
  Task SynchronizeAsync(string entityType, int entityId, Dictionary<string, string?> customAttributes, CancellationToken cancellationToken = default);
}
