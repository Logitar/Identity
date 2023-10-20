using FluentValidation;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents a name part of a person. It can be used to represent, for example, first names, middle names, last names and nicknames.
/// </summary>
public record PersonNameUnit
{
  /// <summary>
  /// The maximum length of person names.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// Gets the value of the person name.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="PersonNameUnit"/> class.
  /// </summary>
  /// <param name="value">The value of the person name.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public PersonNameUnit(string value, string? propertyName = null)
  {
    Value = value.Trim();
    new PersonNameValidator(propertyName).ValidateAndThrow(Value);
  }

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="PersonNameUnit"/> class otherwise.
  /// </summary>
  /// <param name="value">The value of the person name.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static PersonNameUnit? TryCreate(string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value.Trim(), propertyName);
  }
}
