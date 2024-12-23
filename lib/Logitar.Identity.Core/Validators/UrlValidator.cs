using FluentValidation;
using FluentValidation.Validators;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// The validator used to enforce that a string is a Uniform Resource Locators (URL).
/// </summary>
/// <typeparam name="T">The type of the object being validated.</typeparam>
public class UrlValidator<T> : IPropertyValidator<T, string>
{
  /// <summary>
  /// The allowed URL schemes.
  /// </summary>
  private readonly HashSet<string> _schemes = ["http", "https"];

  /// <summary>
  /// Gets the name of the validator.
  /// </summary>
  public string Name { get; } = "UrlValidator";
  /// <summary>
  /// Gets the allowed URL schemes.
  /// </summary>
  public IReadOnlyCollection<string> Schemes => [.. _schemes];

  /// <summary>
  /// Initializes a new instance of the <see cref="UrlValidator{T}"/> class.
  /// </summary>
  /// <param name="schemes">The allowed URL schemes. Defaults to 'http' and 'https'.</param>
  public UrlValidator(IEnumerable<string>? schemes = null)
  {
    if (schemes != null)
    {
      _schemes.Clear();
      foreach (string scheme in schemes)
      {
        _schemes.Add(scheme.ToLowerInvariant());
      }
    }
  }

  /// <summary>
  /// Returns the default error message template for this validator, when not overridden.
  /// </summary>
  /// <param name="errorCode">The error code.</param>
  /// <returns>The default error message template.</returns>
  public string GetDefaultMessageTemplate(string errorCode)
  {
    return $"'{{PropertyName}}' must be a valid absolute Uniform Resource Locators (URL) using one of the following schemes: {string.Join(", ", _schemes)}.";
  }

  /// <summary>
  /// Validates a specific property value.
  /// </summary>
  /// <param name="context">The validation context.</param>
  /// <param name="value">The value to validate.</param>
  /// <returns>True if the value is valid, or false otherwise.</returns>
  public bool IsValid(ValidationContext<T> context, string value)
  {
    try
    {
      Uri uri = new(value, UriKind.Absolute);
      return _schemes.Contains(uri.Scheme.ToLowerInvariant());
    }
    catch (Exception)
    {
      return false;
    }
  }
}
