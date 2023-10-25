﻿using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys;

/// <summary>
/// Represents the identifier of an API key.
/// </summary>
public record ApiKeyId
{
  /// <summary>
  /// Gets the aggregate identifier.
  /// </summary>
  public AggregateId AggregateId { get; }
  /// <summary>
  /// Gets the value of the identifier.
  /// </summary>
  public string Value => AggregateId.Value;

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyId"/> class.
  /// </summary>
  /// <param name="aggregateId">The aggregate identifier.</param>
  public ApiKeyId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyId"/> class.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public ApiKeyId(string value, string? propertyName = null)
  {
    value = value.Trim();
    new AggregateIdValidator(propertyName).ValidateAndThrow(value);

    AggregateId = new(value);
  }

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="ApiKeyId"/> class otherwise.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static ApiKeyId? TryCreate(string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value, propertyName);
  }
}
