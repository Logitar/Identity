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
      .Must(BeAValidUrl)
        .WithErrorCode(nameof(UrlValidator))
        .WithMessage("'{PropertyName}' must be a valid Uniform Resource Locator (URL).")
      .WithPropertyName(propertyName);
  }

  private static bool BeAValidUrl(string uriString)
  {
    try
    {
      _ = new Uri(uriString);
      return true;
    }
    catch (Exception)
    {
      return false;
    }
  }
}
