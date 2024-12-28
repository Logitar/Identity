using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public record EventNotHandled : INotification
{
  public long ExpectedVersion { get; }
  public long ActualVersion { get; }

  public EventNotHandled(long expectedVersion, long actualVersion)
  {
    ArgumentOutOfRangeException.ThrowIfNegative(expectedVersion);
    ArgumentOutOfRangeException.ThrowIfNegative(actualVersion);

    ExpectedVersion = expectedVersion;
    ActualVersion = actualVersion;
  }

  public EventNotHandled(DomainEvent @event, AggregateEntity? aggregate)
  {
    ExpectedVersion = @event.Version - 1;
    ActualVersion = aggregate?.Version ?? 0;
  }
}
