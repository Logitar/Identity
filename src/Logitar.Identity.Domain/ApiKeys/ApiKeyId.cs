using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys;

public record ApiKeyId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public ApiKeyId(AggregateId aggregateId)
  {
    new IdValidator().ValidateAndThrow(aggregateId.Value);

    AggregateId = aggregateId;
  }
  public ApiKeyId(string value)
  {
    value = value.Trim();
    new IdValidator().ValidateAndThrow(value);

    AggregateId = new(value);
  }

  public static ApiKeyId NewId() => new(AggregateId.NewId());
}
