using FluentValidation;
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
  /// <param name="id">The unique identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public ApiKeyId(Guid id, string? propertyName = null) : this(new AggregateId(id), propertyName)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyId"/> class.
  /// </summary>
  /// <param name="aggregateId">The aggregate identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public ApiKeyId(AggregateId aggregateId, string? propertyName = null)
  {
    new IdValidator(propertyName).ValidateAndThrow(aggregateId.Value);

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
    new IdValidator(propertyName).ValidateAndThrow(value);

    AggregateId = new(value);
  }

  /// <summary>
  /// Creates a new API key identifier.
  /// </summary>
  /// <returns>The created identifier.</returns>
  public static ApiKeyId NewId() => new(AggregateId.NewId());

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

  /// <summary>
  /// Converts the identifier to a <see cref="Guid"/>. The conversion will fail if the identifier has not been created from a <see cref="Guid"/>.
  /// </summary>
  /// <returns>The resulting Guid.</returns>
  public Guid ToGuid() => AggregateId.ToGuid();
}
