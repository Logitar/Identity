namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class CustomAttributeEntity
{
  public const int ValueShortenedLength = byte.MaxValue;

  public int CustomAttributeId { get; set; }

  public string EntityType { get; set; } = string.Empty;
  public int EntityId { get; set; }

  public string Key { get; set; } = string.Empty;
  public string Value { get; set; } = string.Empty;
  public string ValueShortened
  {
    get => Value.Truncate(ValueShortenedLength);
    private set { }
  }
}
