using Logitar.Identity.Contracts.Actors;

namespace Logitar.Identity.Contracts;

public abstract class Aggregate
{
  public long Version { get; set; }

  public Actor CreatedBy { get; set; } = new();
  public DateTime CreatedOn { get; set; }

  public Actor UpdatedBy { get; set; } = new();
  public DateTime UpdatedOn { get; set; }
}
