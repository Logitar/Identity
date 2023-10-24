using PhoneNumbers;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Defines extensions methods for phone numbers.
/// See <see cref="PhoneUnit"/> for more information.
/// </summary>
public static class PhoneExtensions
{
  /// <summary>
  /// Validates the specified phone.
  /// </summary>
  /// <param name="phone">The phone to validate.</param>
  /// <param name="defaultRegion">The default <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 alpha-2 country code</see> used for validation. Defaults to US.</param>
  /// <returns>True if the phone is valid, or false otherwise.</returns>
  public static bool IsValid(this IPhone phone, string defaultRegion = "US")
  {
    try
    {
      _ = phone.Parse(defaultRegion);
      return true;
    }
    catch (NumberParseException)
    {
      return false;
    }
  }

  private static PhoneNumber Parse(this IPhone phone, string defaultRegion)
  {
    string formatted = string.IsNullOrWhiteSpace(phone.Extension)
      ? phone.Number : $"{phone.Number} x{phone.Extension}";

    return PhoneNumberUtil.GetInstance().Parse(formatted, phone.CountryCode ?? defaultRegion);
  }
}
