namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class ActorEntity
{
  public int ActorId { get; set; }

  public string Id { get; set; } = string.Empty;
  public string Type { get; set; } = string.Empty;
  public bool IsDeleted { get; set; }

  public string DisplayName { get; set; } = string.Empty;
  public string? EmailAddress { get; set; }
  public string? PictureUrl { get; set; }
}
