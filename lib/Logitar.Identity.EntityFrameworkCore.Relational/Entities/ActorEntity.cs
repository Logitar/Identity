using System.Text;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public sealed class ActorEntity
{
  public int ActorId { get; set; }

  public string Id { get; set; } = string.Empty;
  public string Type { get; set; } = string.Empty;
  public bool IsDeleted { get; set; }

  public string DisplayName { get; set; } = string.Empty;
  public string? EmailAddress { get; set; }
  public string? PictureUrl { get; set; }

  public override bool Equals(object? obj) => obj is ActorEntity actor && actor.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString()
  {
    StringBuilder actor = new();
    actor.Append(DisplayName);
    if (EmailAddress != null)
    {
      actor.Append(" <").Append(EmailAddress).Append('>');
    }
    actor.Append(" (").Append(Type).Append(".Id=").Append(Id).Append(')');
    return actor.ToString();
  }
}
