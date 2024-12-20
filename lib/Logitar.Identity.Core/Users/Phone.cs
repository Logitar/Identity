using FluentValidation;
using Logitar.Identity.Contracts.Users;
using Logitar.Identity.Core.Validators;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents a phone number.
/// </summary>
public record Phone : Contact, IPhone
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
  /// Initializes a new instance of the <see cref="Phone"/> class.
  /// </summary>
  /// <param name="number">The phone number.</param>
  /// <param name="countryCode">The phone country code.</param>
  /// <param name="extension">The phone extension.</param>
  /// <param name="isVerified">A value indicating whether the contact is verified or not.</param>
  public Phone(string number, string? countryCode = null, string? extension = null, bool isVerified = false) : base(isVerified)
  {
    CountryCode = countryCode?.CleanTrim();
    Number = number.Trim();
    Extension = extension?.CleanTrim();
    new PhoneValidator().ValidateAndThrow(this);
  }

  /// <summary>
  /// Returns a string representation of the phone.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => this.FormatToE164();
}
