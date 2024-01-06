using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public static class EventDb
{
  public static class Events
  {
    public static readonly TableId Table = new(nameof(EventContext.Events));

    public static readonly ColumnId AggregateId = new(nameof(EventEntity.AggregateId), Table);
    public static readonly ColumnId AggregateType = new(nameof(EventEntity.AggregateType), Table);
    // TODO(fpion): complete
  }
}
