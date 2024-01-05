using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Sessions;

public record SessionId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public SessionId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  public static SessionId NewId() => new(AggregateId.NewId());
}
