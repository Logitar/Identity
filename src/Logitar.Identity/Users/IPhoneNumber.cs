namespace Logitar.Identity.Users;

public interface IPhoneNumber
{
  /// <summary>
  /// Gets the country code of the phone.
  /// </summary>
  string? CountryCode { get; }

  /// <summary>
  /// Gets the phone number.
  /// </summary>
  string Number { get; }

  /// <summary>
  /// Gets the extension of the phone.
  /// </summary>
  string? Extension { get; }
}
