using FluentValidation;
using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Represents a tenant identifier.
/// </summary>
public record TenantId
{
  /// <summary>
  /// The maximum length of tenant identifiers.
  /// </summary>
  public static readonly int MaximumLength = AggregateId.MaximumLength;

  /// <summary>
  /// Gets the value of the tenant identifier.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="TenantId"/> class.
  /// </summary>
  /// <param name="value">The value of the tenant identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public TenantId(string value, string? propertyName = null)
  {
    Value = value.Trim();
    new TenantIdValidator(propertyName).ValidateAndThrow(Value);
  }

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="TenantId"/> class otherwise.
  /// </summary>
  /// <param name="value">The value of the tenant identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static TenantId? TryCreate(string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value, propertyName);
  }
}
