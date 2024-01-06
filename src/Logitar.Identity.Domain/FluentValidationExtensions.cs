using FluentValidation;

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

  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder) => ruleBuilder.Past(DateTime.Now);
  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime moment)
  {
    return ruleBuilder.Must(value => value < moment)
      .WithErrorCode("PastValidator")
      .WithMessage("'{PropertyName}' must be a date and time set in the past.");
  }
}
