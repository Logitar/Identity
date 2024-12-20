using FluentValidation;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Validators;

namespace Logitar.Identity.Core;

/// <summary>
/// Defines extension methods for input validation.
/// </summary>
public static class ValidationExtensions
{
  /// <summary>
  /// Defines a 'allowed characters' validator on the current rule builder.
  /// Validation will fail if the property contains characters that is not allowed.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="allowedCharacters">The allowed characters.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> AllowedCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, string? allowedCharacters)
  {
    return ruleBuilder.SetValidator(new AllowedCharactersValidator<T>(allowedCharacters));
  }

  // TODO(fpion): CustomIdentifierValue

  /// <summary>
  /// Defines a 'description' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, or only white-space.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Description<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty();
  }

  /// <summary>
  /// Defines a 'display name' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, or its length exceeds the maximum length.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> DisplayName<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.DisplayName.MaximumLength);
  }

  /// <summary>
  /// Defines a 'future' validator on the current rule builder.
  /// Validation will fail if the property is not a date and time set in the future.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="now">The current moment (now) date and time reference.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, DateTime> Future<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? now = null)
  {
    return ruleBuilder.SetValidator(new FutureValidator<T>(now));
  }

  /// <summary>
  /// Defines a 'gender' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, or its length exceeds the maximum length.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Gender<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Users.Gender.MaximumLength);
  }

  /// <summary>
  /// Defines a 'identifier' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, its length exceeds the maximum length, or is not a valid identifier.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Identifier<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.Identifier.MaximumLength).SetValidator(new IdentifierValidator<T>());
  }

  /// <summary>
  /// Defines a 'locale' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, its length exceeds the maximum length, or is not a valid locale.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Locale<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.Locale.MaximumLength).SetValidator(new LocaleValidator<T>());
  }

  /// <summary>
  /// Defines a 'passwprd' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, or does not fulfill the requirements.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="passwordSettings">The password validation settings.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, IPasswordSettings passwordSettings)
  {
    IRuleBuilderOptions<T, string> options = ruleBuilder.NotEmpty();
    if (passwordSettings.RequiredLength > 0)
    {
      options = options.MinimumLength(passwordSettings.RequiredLength)
        .WithErrorCode("PasswordTooShort")
        .WithMessage($"Passwords must be at least {passwordSettings.RequiredLength} characters.");
    }
    if (passwordSettings.RequiredUniqueChars > 0)
    {
      options = options.Must(x => x.GroupBy(c => c).Count() >= passwordSettings.RequiredUniqueChars)
        .WithErrorCode("PasswordRequiresUniqueChars")
        .WithMessage($"Passwords must use at least {passwordSettings.RequiredUniqueChars} different characters.");
    }
    if (passwordSettings.RequireNonAlphanumeric)
    {
      options = options.Must(x => x.Any(c => !char.IsLetterOrDigit(c)))
        .WithErrorCode("PasswordRequiresNonAlphanumeric")
        .WithMessage("Passwords must have at least one non alphanumeric character.");
    }
    if (passwordSettings.RequireLowercase)
    {
      options = options.Must(x => x.Any(char.IsLower))
        .WithErrorCode("PasswordRequiresLower")
        .WithMessage("Passwords must have at least one lowercase ('a'-'z').");
    }
    if (passwordSettings.RequireUppercase)
    {
      options = options.Must(x => x.Any(char.IsUpper))
        .WithErrorCode("PasswordRequiresUpper")
        .WithMessage("Passwords must have at least one uppercase ('A'-'Z').");
    }
    if (passwordSettings.RequireDigit)
    {
      options = options.Must(x => x.Any(char.IsDigit))
        .WithErrorCode("PasswordRequiresDigit")
        .WithMessage("Passwords must have at least one digit ('0'-'9').");
    }
    return options;
  }

  /// <summary>
  /// Defines a 'past' validator on the current rule builder.
  /// Validation will fail if the property is not a date and time set in the past.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="now">The current moment (now) date and time reference.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? now = null)
  {
    return ruleBuilder.SetValidator(new PastValidator<T>(now));
  }

  /// <summary>
  /// Defines a 'person name' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, or its length exceeds the maximum length.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> PersonName<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Users.PersonName.MaximumLength);
  }

  /// <summary>
  /// Defines a 'time zone' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, its length exceeds the maximum length, or is not a valid tz database entry ID.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> TimeZone<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.TimeZone.MaximumLength).SetValidator(new TimeZoneValidator<T>());
  }

  /// <summary>
  /// Defines a 'unique name' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, its length exceeds the maximum length, or contains characters that are not allowed.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="uniqueNameSettings">The unique name validation settings.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> UniqueName<T>(this IRuleBuilder<T, string> ruleBuilder, IUniqueNameSettings uniqueNameSettings)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.UniqueName.MaximumLength).AllowedCharacters(uniqueNameSettings.AllowedCharacters);
  }

  /// <summary>
  /// Defines a 'url' validator on the current rule builder.
  /// Validation will fail if the property is null, an empty string, only white-space, its length exceeds the maximum length, or is not a valid absolute Uniform Resource Locator (URL).
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="schemes">The allowed URL schemes. Defaults to 'http' and 'https'.</param>
  /// <returns>The resulting rule builder options.</returns>
  public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder, IEnumerable<string>? schemes = null)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Core.Url.MaximumLength).SetValidator(new UrlValidator<T>(schemes));
  }
}
