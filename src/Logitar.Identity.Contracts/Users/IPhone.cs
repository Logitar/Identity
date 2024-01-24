namespace Logitar.Identity.Contracts.Users;

/// <summary>
/// Describes phone numbers.
/// </summary>
public interface IPhone
{
  /// <summary>
  /// Gets the <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 alpha-2 country code</see> of the phone.
  /// </summary>
  string? CountryCode { get; }
  /// <summary>
  /// Gets the phone number.
  /// </summary>
  string Number { get; }
  /// <summary>
  /// Gets the phone extension.
  /// </summary>
  string? Extension { get; }
}
