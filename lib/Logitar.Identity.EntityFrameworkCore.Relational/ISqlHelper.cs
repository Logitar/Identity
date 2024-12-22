using Logitar.Data;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public interface ISqlHelper // TODO(fpion): rename
{
  IQueryBuilder QueryFrom(TableId table);
}
