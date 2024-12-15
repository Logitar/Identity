using FluentValidation;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Core;

/// <summary>
/// Represents the unique name of an entity.
/// </summary>
public record UniqueName
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
  /// Initializes a new instance of the <see cref="UniqueName"/> class.
  /// </summary>
  /// <param name="value">The value of the unique name.</param>
  /// <param name="uniqueNameSettings">The unique name settings.</param>
  public UniqueName(string value, IUniqueNameSettings uniqueNameSettings)
  {
    Value = value.Trim();
    new Validator(uniqueNameSettings).ValidateAndThrow(this);
  }

  /// <summary>
  /// Returns a new instance of the <see cref="UniqueName"/> class, or null if the value is null, empty or only white-space.
  /// </summary>
  /// <param name="value">The string value.</param>
  /// <param name="uniqueNameSettings">The unique name settings.</param>
  /// <returns>The new instance, or null.</returns>
  public static UniqueName? TryCreate(string? value, IUniqueNameSettings uniqueNameSettings)
  {
    return string.IsNullOrWhiteSpace(value) ? null : new(value, uniqueNameSettings);
  }

  /// <summary>
  /// Returns a string representation of the unique name.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Value;

  /// <summary>
  /// Represents the validator for instances of <see cref="UniqueName"/>.
  /// </summary>
  private class Validator : AbstractValidator<UniqueName>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    /// <param name="uniqueNameSettings">The unique name settings.</param>
    public Validator(IUniqueNameSettings uniqueNameSettings)
    {
      RuleFor(x => x.Value).UniqueName(uniqueNameSettings);
    }
  }
}
