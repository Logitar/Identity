using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate Uniform Resource Locators (URL).
/// See <see cref="UrlUnit"/> for more information.
/// </summary>
public class UrlValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UrlValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public UrlValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(UrlUnit.MaximumLength)
      .Must(urlString => Uri.IsWellFormedUriString(urlString, UriKind.Absolute))
        .WithErrorCode("UrlValidator")
        .WithMessage("'{PropertyName}' must be a valid Uniform Resource Locator. See https://en.wikipedia.org/wiki/URL for more info.")
      .WithPropertyName(propertyName);
  }
}
