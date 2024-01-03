using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users;

public record UserId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public UserId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  public static UserId NewId() => new(AggregateId.NewId());
}
