using FluentValidation;
using Logitar.Security.Claims;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents the gender of a person.
/// </summary>
public record Gender
{
  /// <summary>
  /// The maximum length of genders.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// The known gender values.
  /// </summary>
  private static readonly HashSet<string> _knownValues = new([Genders.Female, Genders.Male]);
  /// <summary>
  /// Gets the known gender values.
  /// </summary>
  public static IReadOnlyCollection<string> KnownValues => _knownValues.ToList().AsReadOnly();

  /// <summary>
  /// Gets the value of the gender.
  /// </summary>
  public string Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Gender"/> class.
  /// </summary>
  /// <param name="value">The value of the gender.</param>
  public Gender(string value)
  {
    Value = Format(value);
    new Validator().ValidateAndThrow(this);
  }

  /// <summary>
  /// Returns a value indicating whether or not the specified gender is known.
  /// </summary>
  /// <param name="value">The textual representation of the gender.</param>
  /// <returns>True if the gender is known, or false otherwise.</returns>
  public static bool IsKnown(string value) => _knownValues.Contains(value.Trim().ToLower());

  /// <summary>
  /// Returns a new instance of the <see cref="Gender"/> class, or null if the value is null, empty or only white-space.
  /// </summary>
  /// <param name="value">The string value.</param>
  /// <returns>The new instance, or null.</returns>
  public static Gender? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  /// <summary>
  /// Formats the value of a gender.
  /// </summary>
  /// <param name="value">The textual value.</param>
  /// <returns>The formatted value.</returns>
  private static string Format(string value)
  {
    value = value.Trim();
    return IsKnown(value) ? value.ToLower() : value;
  }

  /// <summary>
  /// Returns a string representation of the gender.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Value;

  /// <summary>
  /// Represents the validator for instances of <see cref="Gender"/>.
  /// </summary>
  private class Validator : AbstractValidator<Gender>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Value).Gender();
    }
  }
}
