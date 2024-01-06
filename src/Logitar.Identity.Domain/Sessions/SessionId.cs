using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Sessions;

public record SessionId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public SessionId(AggregateId aggregateId)
  {
    new IdValidator().ValidateAndThrow(aggregateId.Value);

    AggregateId = aggregateId;
  }
  public SessionId(string value)
  {
    value = value.Trim();
    new IdValidator().ValidateAndThrow(value);

    AggregateId = new(value);
  }

  public static SessionId NewId() => new(AggregateId.NewId());
}
