namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public abstract class IdentifierEntity
{
  public string? TenantId { get; protected set; }

  public string Key { get; protected set; } = string.Empty;
  public string Value { get; protected set; } = string.Empty;

  protected IdentifierEntity()
  {
  }
}
