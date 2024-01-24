using FluentValidation;
using Logitar.Identity.Contracts.Settings;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Represents the unique name of an entity.
/// </summary>
public record UniqueNameUnit
{
  /// <summary>
  /// The maximum length of unique names.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// Gets the value of the unique name.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="UniqueNameUnit"/> class.
  /// </summary>
  /// <param name="uniqueNameSettings">The settings used to validate unique names.</param>
  /// <param name="value">The value of the unique name.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public UniqueNameUnit(IUniqueNameSettings uniqueNameSettings, string value, string? propertyName = null)
  {
    Value = value.Trim();
    new UniqueNameValidator(uniqueNameSettings, propertyName).ValidateAndThrow(Value);
  }

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="UniqueNameUnit"/> class otherwise.
  /// </summary>
  /// <param name="uniqueNameSettings">The settings used to validate unique names.</param>
  /// <param name="value">The value of the unique name.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static UniqueNameUnit? TryCreate(IUniqueNameSettings uniqueNameSettings, string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(uniqueNameSettings, value.Trim(), propertyName);
  }
}
