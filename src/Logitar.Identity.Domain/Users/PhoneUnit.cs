using FluentValidation;
using Logitar.Identity.Contracts.Users;
using Logitar.Identity.Domain.Users.Validators;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents a phone number.
/// </summary>
public record PhoneUnit : ContactUnit, IPhone
{
  /// <summary>
  /// The maximum length of phone country codes.
  /// </summary>
  public const int CountryCodeMaximumLength = 2;
  /// <summary>
  /// The maximum length of phone numbers.
  /// </summary>
  public const int NumberMaximumLength = 20;
  /// <summary>
  /// The maximum length of phone extensions.
  /// </summary>
  public const int ExtensionMaximumLength = 10;

  /// <summary>
  /// Gets the <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 alpha-2 country code</see> of the phone.
  /// </summary>
  public string? CountryCode { get; }
  /// <summary>
  /// Gets the phone number.
  /// </summary>
  public string Number { get; }
  /// <summary>
  /// Gets the phone extension.
  /// </summary>
  public string? Extension { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="PhoneUnit"/> class.
  /// </summary>
  /// <param name="number">The phone number.</param>
  /// <param name="countryCode">The <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 alpha-2 country code</see> of the phone.</param>
  /// <param name="extension">The phone extension.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public PhoneUnit(string number, string? countryCode = null, string? extension = null, string? propertyName = null)
    : this(number, countryCode, extension, isVerified: false, propertyName)
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="PhoneUnit"/> class.
  /// </summary>
  /// <param name="number">The phone number.</param>
  /// <param name="countryCode">The <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 alpha-2 country code</see> of the phone.</param>
  /// <param name="extension">The phone extension.</param>
  /// <param name="isVerified">A value indicating whether or not the phone number is verified.</param>
  [JsonConstructor]
  public PhoneUnit(string number, string? countryCode, string? extension, bool isVerified)
    : this(number, countryCode, extension, isVerified, propertyName: null)
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="PhoneUnit"/> class.
  /// </summary>
  /// <param name="number">The phone number.</param>
  /// <param name="countryCode">The <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 alpha-2 country code</see> of the phone.</param>
  /// <param name="extension">The phone extension.</param>
  /// <param name="isVerified">A value indicating whether or not the phone number is verified.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public PhoneUnit(string number, string? countryCode, string? extension, bool isVerified, string? propertyName)
    : base(isVerified)
  {
    CountryCode = countryCode?.CleanTrim();
    Number = number.Trim();
    Extension = extension?.CleanTrim();
    new PhoneValidator(propertyName).ValidateAndThrow(this);
  }
}
