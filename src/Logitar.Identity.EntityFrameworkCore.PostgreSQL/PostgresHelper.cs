﻿using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.Identity.EntityFrameworkCore.Relational;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

public class PostgresHelper : ISqlHelper
{
  public IQueryBuilder QueryFrom(TableId table) => PostgresQueryBuilder.From(table);
}
