using FluentValidation;
using System.Globalization;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate locales.
/// See <see cref="LocaleUnit"/> for more information.
/// </summary>
public class LocaleValidator : AbstractValidator<string>
{
  private const int LOCALE_CUSTOM_UNSPECIFIED = 0x1000;

  /// <summary>
  /// Initializes a new instance of the <see cref="LocaleValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public LocaleValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .Must(BeAValidLocale)
        .WithErrorCode(nameof(LocaleValidator))
        .WithMessage("'{PropertyName}' may not be the invariant culture, nor an user-defined culture.")
      .WithPropertyName(propertyName);
  }

  private static bool BeAValidLocale(string cultureName)
  {
    try
    {
      CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);

      return !string.IsNullOrEmpty(culture.Name) && culture.LCID != LOCALE_CUSTOM_UNSPECIFIED;
    }
    catch (Exception)
    {
      return false;
    }
  }
}
