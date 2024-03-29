﻿using Logitar.Data;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public static class QueryingExtensions
{
  public static IQueryable<T> FromQuery<T>(this DbSet<T> entities, IQueryBuilder builder) where T : class
    => entities.FromQuery(builder.Build());
  public static IQueryable<T> FromQuery<T>(this DbSet<T> entities, IQuery query) where T : class
    => entities.FromSqlRaw(query.Text, query.Parameters.ToArray());
}
