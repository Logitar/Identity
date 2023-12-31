﻿namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// Defines shared properties of objects with custom attributes.
/// </summary>
internal interface ICustomAttributes
{
  /// <summary>
  /// Gets the aggregate identifier of the object.
  /// </summary>
  string AggregateId { get; }

  /// <summary>
  /// Gets the serialized custom attributes of the object.
  /// </summary>
  string? CustomAttributes { get; }
}
