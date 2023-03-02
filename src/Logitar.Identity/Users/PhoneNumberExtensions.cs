using PhoneNumbers;
using System.Text;

namespace Logitar.Identity.Users;

/// <summary>
/// Defines extension methods for phone numbers.
/// </summary>
public static class PhoneNumberExtensions
{
  /// <summary>
  /// The default region (ISO 3166-1 alpha-2 country code) of phone numbers.
  /// </summary>
  private const string DefaultRegion = "US";

  /// <summary>
  /// Returns a value indicating whether or not the specified phone number is valid.
  /// </summary>
  /// <param name="phoneNumber">The phone number to validate.</param>
  /// <returns>True if the phone number is valid, false otherwise.</returns>
  public static bool IsValid(this IPhoneNumber phoneNumber)
  {
    try
    {
      _ = phoneNumber.Parse();

      return true;
    }
    catch (NumberParseException)
    {
      return false;
    }
  }

  /// <summary>
  /// Returns a string representation of the specified phone number in the E.164 format.
  /// </summary>
  /// <param name="phoneNumber">The phone number to format.</param>
  /// <returns>The string representation.</returns>
  public static string ToE164String(this IPhoneNumber phoneNumber)
  {
    PhoneNumber phone = phoneNumber.Parse();

    return PhoneNumberUtil.GetInstance().Format(phone, PhoneNumberFormat.E164);
  }

  /// <summary>
  /// Parses the specified phone number.
  /// </summary>
  /// <param name="phoneNumber">The phone number to parse.</param>
  /// <returns>The parsed phone number.</returns>
  private static PhoneNumber Parse(this IPhoneNumber phoneNumber)
  {
    StringBuilder phone = new();

    if (!string.IsNullOrEmpty(phoneNumber.CountryCode))
    {
      phone.Append(phoneNumber.CountryCode);
      phone.Append(' ');
    }

    phone.Append(phoneNumber.Number);

    if (!string.IsNullOrEmpty(phoneNumber.Extension))
    {
      phone.Append(" x");
      phone.Append(phoneNumber.Extension);
    }

    return PhoneNumberUtil.GetInstance().Parse(phone.ToString(), DefaultRegion);
  }
}
