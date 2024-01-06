using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

public record UserId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public UserId(AggregateId aggregateId)
  {
    new IdValidator().ValidateAndThrow(aggregateId.Value);
    AggregateId = aggregateId;
  }

  public static UserId NewId() => new(AggregateId.NewId());
}
