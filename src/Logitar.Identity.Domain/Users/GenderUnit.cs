using FluentValidation;
using Logitar.Security.Claims;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents the gender of a person.
/// </summary>
public record GenderUnit
{
  /// <summary>
  /// The maximum length of user genders.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  private static readonly HashSet<string> _knownValues = new(new[] { Genders.Female, Genders.Male });

  /// <summary>
  /// Gets the value of the gender.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="GenderUnit"/> class.
  /// </summary>
  /// <param name="value">The value of the gender.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public GenderUnit(string value, string? propertyName = null)
  {
    Value = (IsKnown(value) ? value.ToLower() : value).Trim();
    new GenderValidator(propertyName).ValidateAndThrow(Value);
  }

  /// <summary>
  /// Returns a value indicating whether or not the specified gender is known.
  /// </summary>
  /// <param name="value">The textual representation of the gender.</param>
  /// <returns>True if the gender is known, or false otherwise.</returns>
  public static bool IsKnown(string value) => _knownValues.Contains(value.Trim().ToLower());

  /// <summary>
  /// Returns null if the input is empty, or a new instance of the <see cref="GenderUnit"/> class otherwise.
  /// </summary>
  /// <param name="value">The value of the gender.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  /// <returns>The created instance or null.</returns>
  public static GenderUnit? TryCreate(string? value, string? propertyName = null)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value.Trim(), propertyName);
  }
}
