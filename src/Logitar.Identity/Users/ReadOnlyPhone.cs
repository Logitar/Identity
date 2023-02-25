using Logitar.EventSourcing;

namespace Logitar.Identity.Users;

/// <summary>
/// Represents the phone number of an user.
/// </summary>
public record ReadOnlyPhone : ReadOnlyContact, IPhoneNumber
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyPhone"/> record using the specified arguments.
  /// </summary>
  /// <param name="countryCode">The country code of the phone.</param>
  /// <param name="number">The phone number.</param>
  /// <param name="extension">The extension of the phone.</param>
  /// <param name="isVerified">A value indicating whether or not the phone number is verified.</param>
  public ReadOnlyPhone(string number,
    string? countryCode = null,
    string? extension = null,
    bool isVerified = false) : base(isVerified)
  {
    CountryCode = countryCode?.CleanTrim();
    Number = number.Trim();
    Extension = extension?.CleanTrim();
  }

  /// <summary>
  /// Gets the country code of the phone.
  /// </summary>
  public string? CountryCode { get; }

  /// <summary>
  /// Gets the phone number.
  /// </summary>
  public string Number { get; }

  /// <summary>
  /// Gets the extension of the phone.
  /// </summary>
  public string? Extension { get; }
}
