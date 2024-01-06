using FluentValidation;
using NodaTime;

namespace Logitar.Identity.Domain;

public static class FluentValidationExtensions
{
  public static IRuleBuilderOptions<T, string> AllowedCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, string? allowedCharacters)
  {
    return ruleBuilder.Must(s => allowedCharacters == null || s.All(allowedCharacters.Contains))
      .WithErrorCode("AllowedCharactersValidator")
      .WithMessage($"'{{PropertyName}}' may only include the following characters: {allowedCharacters}");
  }

  public static IRuleBuilderOptions<T, DateTime> Future<T>(this IRuleBuilder<T, DateTime> ruleBuilder) => ruleBuilder.Past(DateTime.Now);
  public static IRuleBuilderOptions<T, DateTime> Future<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime moment)
  {
    return ruleBuilder.Must(value => value > moment)
      .WithErrorCode("PastValidator")
      .WithMessage("'{PropertyName}' must be a date and time set in the future.");
  }

  private const int LOCALE_CUSTOM_UNSPECIFIED = 0x1000;
  public static IRuleBuilderOptions<T, string> Locale<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidLocale)
      .WithErrorCode("LocaleValidator")
      .WithMessage("'{PropertyName}' may not be the invariant culture, nor a user-defined culture.");
  }
  private static bool BeAValidLocale(string name)
  {
    try
    {
      CultureInfo culture = CultureInfo.GetCultureInfo(name);
      return !string.IsNullOrEmpty(culture.Name) && culture.LCID != LOCALE_CUSTOM_UNSPECIFIED;
    }
    catch (CultureNotFoundException)
    {
      return false;
    }
  }

  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder) => ruleBuilder.Past(DateTime.Now);
  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime moment)
  {
    return ruleBuilder.Must(value => value < moment)
      .WithErrorCode("PastValidator")
      .WithMessage("'{PropertyName}' must be a date and time set in the past.");
  }

  public static IRuleBuilderOptions<T, string> TimeZone<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(id => DateTimeZoneProviders.Tzdb.GetZoneOrNull(id) != null)
      .WithErrorCode("TimeZoneValidator")
      .WithMessage("'{PropertyName}' did not resolve to a tz entry.");
  }
}
