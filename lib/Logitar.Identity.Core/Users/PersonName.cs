using FluentValidation;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents a name part of a person. It can be used to represent, for example, first names, middle names, last names and nicknames.
/// </summary>
public record PersonName
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
  /// Initializes a new instance of the <see cref="PersonName"/> class.
  /// </summary>
  /// <param name="value">The value of the person name.</param>
  public PersonName(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  /// <summary>
  /// Builds the full name of a person from its specified list of names.
  /// </summary>
  /// <param name="names">The list of names.</param>
  /// <returns>The full name.</returns>
  public static string? BuildFullName(params PersonName?[] names) => BuildFullName(names.Select(name => name?.Value).ToArray());
  /// <summary>
  /// Builds the full name of a person from its specified list of names.
  /// </summary>
  /// <param name="names">The list of names.</param>
  /// <returns>The full name.</returns>
  public static string? BuildFullName(params string?[] names) => string.Join(' ', names
    .SelectMany(name => name?.Split(' ') ?? [])
    .Where(name => !string.IsNullOrEmpty(name))).CleanTrim();

  /// <summary>
  /// Returns a new instance of the <see cref="PersonName"/> class, or null if the value is null, empty or only white-space.
  /// </summary>
  /// <param name="value">The string value.</param>
  /// <returns>The new instance, or null.</returns>
  public static PersonName? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  /// <summary>
  /// Returns a string representation of the person name.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Value;

  /// <summary>
  /// Represents the validator for instances of <see cref="PersonName"/>.
  /// </summary>
  private class Validator : AbstractValidator<PersonName>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Value).PersonName();
    }
  }
}
