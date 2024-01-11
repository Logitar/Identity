using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Roles;

public record RoleId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public RoleId(AggregateId aggregateId)
  {
    new IdValidator().ValidateAndThrow(aggregateId.Value);

    AggregateId = aggregateId;
  }
  public RoleId(string value)
  {
    value = value.Trim();
    new IdValidator().ValidateAndThrow(value);

    AggregateId = new(value);
  }

  public static RoleId NewId() => new(AggregateId.NewId());
}
