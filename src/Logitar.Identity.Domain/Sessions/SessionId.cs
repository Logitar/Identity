using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Sessions;

/// <summary>
/// Represents the identifier of a session.
/// </summary>
public record SessionId
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
  /// Initializes a new instance of the <see cref="SessionId"/> class.
  /// </summary>
  /// <param name="aggregateId">The aggregate identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public SessionId(AggregateId aggregateId, string? propertyName = null)
  {
    new IdValidator(propertyName).ValidateAndThrow(aggregateId.Value);

    AggregateId = aggregateId;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionId"/> class.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public SessionId(string value, string? propertyName = null)
  {
    value = value.Trim();
    new IdValidator(propertyName).ValidateAndThrow(value);

    AggregateId = new(value);
  }

  /// <summary>
  /// Creates a new session identifier.
  /// </summary>
  /// <returns>The created identifier.</returns>
  public static SessionId NewId() => new(AggregateId.NewId());

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="SessionId"/> class otherwise.
  /// </summary>
  /// <param name="value">The value of the identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static SessionId? TryCreate(string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value, propertyName);
  }
}
